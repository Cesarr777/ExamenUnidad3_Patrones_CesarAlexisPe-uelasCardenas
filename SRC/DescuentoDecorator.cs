using System;

namespace HotelReservaApp
{
    public class DescuentoDecorator : ProductoDecorator
    {
        private readonly double _porc;
        public DescuentoDecorator(HabitacionBase inner, double porcentaje) : base(inner) { _porc = porcentaje; }
        public override double Precio => Math.Round(_inner.Precio * (1 - _porc / 100.0), 2);
        public override string Descripcion => _inner.Descripcion + $" - Descuento {_porc}% aplicado";
    }
}
