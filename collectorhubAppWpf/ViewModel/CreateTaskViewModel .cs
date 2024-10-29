using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MVVM.Commands;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using collectorhubAppWpf.Model;
using System.Windows.Controls;
using collectorhubAppWpf.View;
using collectorhubAppWpf.Stores;
using System.Collections.ObjectModel;

namespace collectorhubAppWpf.ViewModel
{
    public class CreateTaskViewModel : ViewModelBase
    {
        private string _taskName;
        private string _taskDescription;
        private string _selectedTaskType;

        public ObservableCollection<string> TaskTypes { get; set; }

        public string TaskName
        {
            get => _taskName;
            set { _taskName = value; OnPropertyChanged(nameof(TaskName)); }
        }

        public string TaskDescription
        {
            get => _taskDescription;
            set { _taskDescription = value; OnPropertyChanged(nameof(TaskDescription)); }
        }

        public string SelectedTaskType
        {
            get => _selectedTaskType;
            set { _selectedTaskType = value; OnPropertyChanged(nameof(SelectedTaskType)); }
        }

        public ICommand CreateTaskCommand { get; }

        private readonly HttpClient _httpClient;

        public CreateTaskViewModel()
        {
            // Inicializamos la lista de tipos de tarea
            TaskTypes = new ObservableCollection<string>
            {
                "General", "Diaria", "Semanal", "Mensual", "Especial"
            };

            // Comando para crear la tarea
            CreateTaskCommand = new RelayCommand(param => CreateTask());

            // Inicializa el HttpClient
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/") // Cambia la URL base según sea necesario
            };
        }

        private async void CreateTask()
        {
            // Creamos un objeto de solicitud de tarea
            var taskRequest = new
            {
                description = TaskDescription, // Cambiar a minúscula
                isCompleted = false, // La tarea no está completada al crearla
                title = TaskName, // Cambiar a minúscula
                taskType = SelectedTaskType // Cambiar a minúscula
            };

            // Convertimos el objeto a JSON
            var json = JsonConvert.SerializeObject(taskRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Realizamos la solicitud POST para crear la tarea
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                var response = await _httpClient.PostAsync("/task/new", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tarea creada con éxito.");
                    // Aquí puedes limpiar los campos o manejar el éxito de otra manera
                }
                else
                {
                    MessageBox.Show("Error al crear la tarea: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }
    }
}
