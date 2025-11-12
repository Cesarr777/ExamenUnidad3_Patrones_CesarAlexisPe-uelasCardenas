using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReservaApp
{
    public class Calendario
    {
        private readonly List<Dia> _dias = new();
        public string Nombre { get; }

        private Calendario(string nombre)
        {
            Nombre = nombre;
        }

        public IEnumerable<Dia> Dias => _dias;

        public static Calendario CrearCalendario(string nombre, int diasIniciales = 60)
        {
            var c = new Calendario(nombre);
            for (int i = 0; i < diasIniciales; i++) c._dias.Add(new Dia(DateTime.Today.AddDays(i)));
            return c;
        }

        public string Estado()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Calendario: {Nombre}");
            sb.AppendLine("Leyenda: [D]=Disponible [R]=Reservado [O]=Ocupado\n");
            foreach (var d in _dias) sb.AppendLine(d.Estado());
            return sb.ToString();
        }

        public bool EstaDisponible(DateTime start, int nights)
        {
            for (int i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var dia = _dias.FirstOrDefault(x => x.Fecha == date);
                if (dia != null && dia.EstadoActual != EstadoDia.Disponible) return false;
            }
            return true;
        }

        public bool Reservar(DateTime start, int nights, ReservaInfo info)
        {
            if (!EstaDisponible(start, nights)) return false;
            for (int i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var dia = _dias.FirstOrDefault(x => x.Fecha == date);
                if (dia != null)
                {
                    dia.EstadoActual = EstadoDia.Reservado;
                    dia.InfoReserva = info;
                }
                else
                {
                    var nuevo = new Dia(date) { EstadoActual = EstadoDia.Reservado, InfoReserva = info };
                    _dias.Add(nuevo);
                }
            }
            return true;
        }

        public bool Ocupar(DateTime start, int nights, ReservaInfo? info = null)
        {
            for (int i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var dia = _dias.FirstOrDefault(x => x.Fecha == date);
                if (dia != null)
                {
                    if (dia.EstadoActual == EstadoDia.Reservado && dia.InfoReserva != null && info != null && !string.Equals(dia.InfoReserva.Email, info.Email, StringComparison.OrdinalIgnoreCase))
                        return false;
                    dia.EstadoActual = EstadoDia.Ocupado;
                    dia.InfoReserva = info ?? dia.InfoReserva;
                }
                else
                {
                    _dias.Add(new Dia(date) { EstadoActual = EstadoDia.Ocupado, InfoReserva = info });
                }
            }
            return true;
        }

        public void Liberar(DateTime start, int nights)
        {
            for (int i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var dia = _dias.FirstOrDefault(x => x.Fecha == date);
                if (dia != null)
                {
                    dia.EstadoActual = EstadoDia.Disponible;
                    dia.InfoReserva = null;
                }
            }
        }

        public bool Cancelar(DateTime start, int nights, string email)
        {
            bool any = false;
            for (int i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var dia = _dias.FirstOrDefault(x => x.Fecha == date);
                if (dia != null && dia.InfoReserva != null && string.Equals(dia.InfoReserva.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    dia.EstadoActual = EstadoDia.Disponible;
                    dia.InfoReserva = null;
                    any = true;
                }
            }
            return any;
        }

        public ReservaInfo? ObtenerReserva(DateTime fecha) => _dias.FirstOrDefault(x => x.Fecha == fecha.Date)?.InfoReserva;
    }
}
