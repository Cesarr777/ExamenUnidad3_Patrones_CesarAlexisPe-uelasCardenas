namespace HotelReservaApp
{
    public abstract class ProductoDecorator : HabitacionBase
    {
        protected HabitacionBase _inner;
        protected ProductoDecorator(HabitacionBase inner) { _inner = inner; }

        public override string Nombre => _inner.Nombre;
        public override string Descripcion => _inner.Descripcion;
        public override double Precio => _inner.Precio;
        public override Calendario ReservaCalendario => _inner.ReservaCalendario;
    }
}
