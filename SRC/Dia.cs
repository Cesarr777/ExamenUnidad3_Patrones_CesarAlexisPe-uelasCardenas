using System;

namespace HotelReservaApp
{
    public enum EstadoDia { Disponible, Reservado, Ocupado }

    public class Dia
    {
        public DateTime Fecha { get; }
        public EstadoDia EstadoActual { get; set; } = EstadoDia.Disponible;
        public ReservaInfo? InfoReserva { get; set; }

        public Dia(DateTime fecha)
        {
            Fecha = fecha.Date;
        }

        public string Estado()
        {
            string simbolo = EstadoActual switch
            {
                EstadoDia.Disponible => "[D]",
                EstadoDia.Reservado => "[R]",
                EstadoDia.Ocupado => "[O]",
                _ => "[?]"
            };

            var info = InfoReserva != null ? $" ({InfoReserva.Nombre})" : string.Empty;
            return $"{Fecha:yyyy-MM-dd} {simbolo}{info}";
        }
    }
}
