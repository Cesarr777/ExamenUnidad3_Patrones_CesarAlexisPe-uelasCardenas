namespace HotelReservaApp
{
    public abstract class HabitacionBase
    {
        public abstract string Nombre { get; }
        public abstract string Descripcion { get; }
        public abstract double Precio { get; }

        public abstract Calendario ReservaCalendario { get; }
    }
}
