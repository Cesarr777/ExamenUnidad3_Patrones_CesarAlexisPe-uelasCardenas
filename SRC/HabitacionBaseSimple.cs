using System;

namespace HotelReservaApp
{
    public class HabitacionBaseSimple : HabitacionBase
    {
        private readonly string _nombre;
        private readonly string _descripcion;
        private readonly double _precio;
        private Calendario? _calendario;

        public HabitacionBaseSimple(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    _nombre = "Base - Vista al jardín";
                    _descripcion = "Habitacion sencilla con vista al jardín.";
                    _precio = 1500.0;
                    break;
                case 2:
                    _nombre = "Base - Balcón";
                    _descripcion = "Habitacion base con balcón";
                    _precio = 1800.0;
                    break;
                case 3:
                    _nombre = "Base - Estándar";
                    _descripcion = "Habitacion base estándar";
                    _precio = 1200.0;
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
