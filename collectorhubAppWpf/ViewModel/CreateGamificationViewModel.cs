using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // Para el OpenFileDialog
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
        // Propiedades de la gamificación
        private string _gamificationTitle;
        private string _gamificationDescription;
        private string _gamificationImageUrl;
        private ObservableCollection<GamificationConditionModel> _gamificationConditions;

        public CreateGamificationViewModel()
        {
            GamificationConditions = new ObservableCollection<GamificationConditionModel>();
            AddConditionCommand = new RelayCommand(param => AddCondition());
            RemoveConditionCommand = new RelayCommand(param => RemoveCondition(param));
            UploadImageCommand = new RelayCommand(param => UploadImage());
            CreateGamificationCommand = new RelayCommand(param => CreateGamification());
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
            GamificationConditions.Add(new GamificationConditionModel()); // Añade una nueva condición vacía
        }

        private void RemoveCondition(object parameter)
        {
            if (parameter is GamificationConditionModel condition && GamificationConditions.Contains(condition))
            {
                GamificationConditions.Remove(condition); // Elimina la condición seleccionada
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
                // Usar HttpClient para subir la imagen al servidor
                using (HttpClient client = new HttpClient())
                {
                    // Crear un MultipartFormDataContent para enviar el archivo
                    using (var form = new MultipartFormDataContent())
                    {
                        // Leer el archivo seleccionado
                        var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Cambia según el tipo de imagen

                        // Añadir el contenido del archivo al formulario
                        form.Add(fileContent, "file", Path.GetFileName(openFileDialog.FileName));

                        try
                        {
                            // Hacer la solicitud POST al endpoint de subida de imágenes
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                            var response = await client.PostAsync("http://localhost:8080/gamification/upload-image", form);

                            if (response.IsSuccessStatusCode)
                            {
                                // Obtener la URL de la imagen desde la respuesta
                                var imageUrl = await response.Content.ReadAsStringAsync();
                                GamificationImageUrl = imageUrl; // Actualizar la propiedad con la URL devuelta
                            }
                            else
                            {
                                // Manejo de errores en caso de fallo
                                MessageBox.Show("Error al subir la imagen: " + response.ReasonPhrase);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Manejo de excepciones
                            MessageBox.Show("Excepción al subir la imagen: " + ex.Message);
                        }
                        finally
                        {
                            fileStream.Close(); // Asegurarte de cerrar el flujo
                        }
                    }
                }

                // Notificar a la vista que ha cambiado
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




    }
}
