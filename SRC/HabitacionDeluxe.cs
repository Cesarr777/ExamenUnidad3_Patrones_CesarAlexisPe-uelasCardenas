using System;

namespace HotelReservaApp
{
    public class HabitacionDeluxe : HabitacionBase
    {
        private readonly string _nombre;
        private readonly string _descripcion;
        private readonly double _precio;
        private Calendario? _calendario;

        public HabitacionDeluxe(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    _nombre = "Deluxe - Vista al mar";
                    _descripcion = "Habitación deluxe con amplias ventanas y balcón.";
                    _precio = 3000.0;
                    break;
                case 2:
                    _nombre = "Deluxe - Balcón";
                    _descripcion = "Habitación deluxe con balcón privado.";
                    _precio = 4000.0;
                    break;
                case 3:
                    _nombre = "Deluxe - Jacuzzi";
                    _descripcion = "Habitación deluxe con jacuzzi privado.";
                    _precio = 4500.0;
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
