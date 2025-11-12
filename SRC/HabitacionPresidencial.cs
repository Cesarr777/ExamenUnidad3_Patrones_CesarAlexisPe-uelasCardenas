using System;

namespace HotelReservaApp
{
    public class HabitacionPresidencial : HabitacionBase
    {
        private readonly string _nombre;
        private readonly string _descripcion;
        private readonly double _precio;
        private Calendario? _calendario;

        public HabitacionPresidencial(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    _nombre = "Presidencial - Vista al mar";
                    _descripcion = "Suite presidencial con múltiples habitaciones.";
                    _precio = 6000.0;
                    break;
                case 2:
                    _nombre = "Presidencial - Balcón privado";
                    _descripcion = "Suite presidencial con balcón privado.";
                    _precio = 7000.0;
                    break;
                case 3:
                    _nombre = "Presidencial - Jacuzzi";
                    _descripcion = "Suite presidencial con jacuzzi y sala de estar.";
                    _precio = 7500.0;
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
