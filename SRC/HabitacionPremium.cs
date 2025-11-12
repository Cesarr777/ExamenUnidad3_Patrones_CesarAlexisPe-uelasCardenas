using System;

namespace HotelReservaApp
{
    public class HabitacionPremium : HabitacionBase
    {
        private readonly string _nombre;
        private readonly string _descripcion;
        private readonly double _precio;
        private Calendario? _calendario;

        public HabitacionPremium(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    _nombre = "Premium - Vista al mar";
                    _descripcion = "Habitación premium para dos personas con vista.";
                    _precio = 2500.0;
                    break;
                case 2:
                    _nombre = "Premium - Balcón";
                    _descripcion = "Premium con balcón privado.";
                    _precio = 3000.0;
                    break;
                case 3:
                    _nombre = "Premium - Jacuzzi";
                    _descripcion = "Premium con jacuzzi.";
                    _precio = 3500.0;
                    break;
                default:
                    throw new ArgumentException("Opción inválida");
            }
        }

        public override string Nombre => _nombre;
        public override string Descripcion => _descripcion;
        public override double Precio => _precio;

        public override Calendario ReservaCalendario => _calendario ??= Calendario.CrearCalendario(Nombre, 90);
    }
}
