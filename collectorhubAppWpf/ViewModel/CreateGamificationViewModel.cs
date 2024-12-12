using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using collectorhubAppWpf.Model;
using MVVM.Commands;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace collectorhubAppWpf.ViewModel
{
    public class CreateGamificationViewModel : ViewModelBase
    {
        private string _gamificationTitle;
        private string _gamificationDescription;
        private string _gamificationImageUrl;
        private ObservableCollection<GamificationConditionModel> _gamificationConditions;
        public ObservableCollection<string> TaskTypes { get; }


        public CreateGamificationViewModel()
        {
            TaskTypes = new ObservableCollection<string>
        {
            "General", "Diaria", "Semanal", "Mensual", "Especial"
        };

            GamificationConditions = new ObservableCollection<GamificationConditionModel>();
            AddConditionCommand = new RelayCommand(param => AddCondition());
            RemoveConditionCommand = new RelayCommand(param => RemoveCondition(param));
            UploadImageCommand = new RelayCommand(param => UploadImage());
            CreateGamificationCommand = new RelayCommand(param => CreateGamification(), param => AreFieldsValid());
        }

        public string GamificationTitle
        {
            get => _gamificationTitle;
            set
            {
                _gamificationTitle = value;
                OnPropertyChanged(nameof(GamificationTitle));
            }
        }

        public string GamificationDescription
        {
            get => _gamificationDescription;
            set
            {
                _gamificationDescription = value;
                OnPropertyChanged(nameof(GamificationDescription));
            }
        }

        public string GamificationImageUrl
        {
            get => _gamificationImageUrl;
            set
            {
                _gamificationImageUrl = value;
                OnPropertyChanged(nameof(GamificationImageUrl));
            }
        }

        public ObservableCollection<GamificationConditionModel> GamificationConditions
        {
            get => _gamificationConditions;
            set
            {
                _gamificationConditions = value;
                OnPropertyChanged(nameof(GamificationConditions));
            }
        }

        public ICommand AddConditionCommand { get; }
        public ICommand RemoveConditionCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ICommand CreateGamificationCommand { get; }

        private void AddCondition()
        {
            GamificationConditions.Add(new GamificationConditionModel());
        }

        private void RemoveCondition(object parameter)
        {
            if (parameter is GamificationConditionModel condition && GamificationConditions.Contains(condition))
            {
                GamificationConditions.Remove(condition);
            }
        }

        private async void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                        form.Add(fileContent, "file", Path.GetFileName(openFileDialog.FileName));

                        try
                        {
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                            var response = await client.PostAsync("http://localhost:8080/gamification/upload-image", form);

                            if (response.IsSuccessStatusCode)
                            {
                                var imageUrl = await response.Content.ReadAsStringAsync();
                                GamificationImageUrl = imageUrl;
                            }
                            else
                            {
                                MessageBox.Show("Error al subir la imagen: " + response.ReasonPhrase);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Excepción al subir la imagen: " + ex.Message);
                        }
                        finally
                        {
                            fileStream.Close();
                        }
                    }
                }

                OnPropertyChanged(nameof(GamificationImageUrl));
            }
        }


        private async void CreateGamification()
        {
            var gamificationRequest = new GamificationModel
            {
                Title = GamificationTitle,
                Description = GamificationDescription,
                ImageUrl = GamificationImageUrl,
                Conditions = GamificationConditions.Select(c => new GamificationConditionModel
                {
                    Type = c.Type,
                    Threshold = c.Threshold
                }).ToList()
            };

            var json = JsonConvert.SerializeObject(gamificationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                    var response = await client.PostAsync("http://localhost:8080/gamification/new", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Gamificación creada con éxito.");
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Error al crear la gamificación: " + response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excepción al crear la gamificación: " + ex.Message);
                }
            }
        }

        private bool AreFieldsValid()
        {
            return !string.IsNullOrWhiteSpace(GamificationTitle) &&
                   !string.IsNullOrWhiteSpace(GamificationDescription) &&
                   !string.IsNullOrWhiteSpace(GamificationImageUrl);
        }

        private void ClearFields()
        {
            GamificationTitle = string.Empty;
            GamificationDescription = string.Empty;
            GamificationImageUrl = string.Empty;
            GamificationConditions.Clear();
        }

    }
}
