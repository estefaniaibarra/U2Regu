using Client.Models;
using Client.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModel
{
    
        public class VotacionViewModel : INotifyPropertyChanged
        {
            VotacionesCliente cliente = new VotacionesCliente();

            public Pregunta Pregunta { get; set; }
            public string Error { get; set; } = "";
            public ICommand VotarCommand { get; set; }
            public bool Votando { get; set; }

            public VotacionViewModel()
            {
                VotarCommand = new Command<int>(Votar);
                CargarPregunta();
            }

            async void CargarPregunta()
            {
                Pregunta = await cliente.GetPregunta();
                if (Pregunta == null)
                {
                    Error = "No se conectó al servidor";

                }
                Actualizar();
            }

            private async void Votar(int obj)
            {
                try
                {
                    if (Pregunta != null)
                    {
                        Voto v = new Voto();
                        v.Opcion = obj;
                        await cliente.VotarPost(v);
                        Votando = false;
                        Actualizar(nameof(Votando));
                    }
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    Actualizar(nameof(Error));
                }
            }

            void Actualizar(string property = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }

