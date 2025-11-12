using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservaApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalogo = new Dictionary<int, Func<int, HabitacionBase>>
            {
                [1] = (op) => new HabitacionDeluxe(op),
                [2] = (op) => new HabitacionPremium(op),
                [3] = (op) => new HabitacionBaseSimple(op),
                [4] = (op) => new HabitacionPresidencial(op)
            };

            var carrito = new List<(HabitacionBase room, ReservaInfo? reserva)>();
            var rng = new Random();

            var inventory = new Dictionary<int, Dictionary<int, HabitacionBase>>();
            for (int c = 1; c <= 4; c++)
            {
                inventory[c] = new Dictionary<int, HabitacionBase>();
                for (int o = 1; o <= 3; o++) inventory[c][o] = catalogo[c](o);
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("SISTEMA DE RESERVAS\n");
                Console.WriteLine("Categorías:");
                Console.WriteLine("1) Deluxe\n2) Premium\n3) Base\n4) Presidencial\n5) Salir");
                Console.Write("Elige categoría: ");
                if (!int.TryParse(Console.ReadLine(), out var cat) || cat < 1 || cat > 5) continue;
                if (cat == 5) break;

                Console.Clear();
                Console.WriteLine("Elige variante:");
                for (int o = 1; o <= 3; o++)
                {
                    var tmp = inventory[cat][o];
                    Console.WriteLine($"{o}) {tmp.Nombre} - ${tmp.Precio:F2}");
                }
                Console.Write("Opción: ");
                if (!int.TryParse(Console.ReadLine(), out var op) || op < 1 || op > 3) continue;

                var habitacion = inventory[cat][op];

                bool backToMenu = false;
                bool exitProgram = false;
                while (!backToMenu)
                {
                    Console.Clear();
                    Console.WriteLine($"Habitación: {habitacion.Nombre}");
                    Console.WriteLine(habitacion.Descripcion);
                    Console.WriteLine($"Precio base: ${habitacion.Precio:F2}\n");

                    Console.WriteLine(habitacion.ReservaCalendario.Estado());

                    Console.WriteLine("Acciones:\n1) Reservar\n2) Liberar (solo si está reservado)\n3) Reservar otro\n4) Pagar\n5) Volver al menú categorías");
                    Console.Write("Acción: ");
                    var act = Console.ReadLine() ?? string.Empty;

                    switch (act)
                    {
                        case "1":
                            var info = PedirDatosReserva(minimalMode: true);
                            if (info == null) { Console.WriteLine("Datos inválidos."); Pause(); break; }
                            if (!ValidadorReservas.ValidarFechaReserva(info.Start, out var m1)) { Console.WriteLine(m1); Pause(); break; }
                            if (!ValidadorReservas.ValidarDuracionEstancia(info.Nights, out var m2)) { Console.WriteLine(m2); Pause(); break; }
                            if (info.Start.DayOfWeek == DayOfWeek.Saturday || info.Start.DayOfWeek == DayOfWeek.Sunday)
                            {
                                info.DiscountPercent = 10.0;
                                info.DiscountReason = "Festivo";
                            }
                            else if (rng.Next(0, 2) == 1)
                            {
                                info.DiscountPercent = 10.0;
                                info.DiscountReason = "Local";
                            }

                            if (!habitacion.ReservaCalendario.Reservar(info.Start, info.Nights, info)) { Console.WriteLine("Fechas no disponibles."); Pause(); break; }
                            var exists = carrito.Any(x => ReferenceEquals(x.room, habitacion) && x.reserva != null && x.reserva.Start == info.Start && string.Equals(x.reserva.Email, info.Email, StringComparison.OrdinalIgnoreCase));
                            if (!exists)
                            {
                                carrito.Add((habitacion, info));
                            }
                            Console.WriteLine("Reserva realizada con éxito.");
                            Console.WriteLine("¿Qué deseas hacer ahora?\n1) Reservar otra habitación\n2) Pagar ahora\n3) Volver");
                            Console.Write("Opción: ");
                            var post = Console.ReadLine() ?? string.Empty;
                            if (post == "1") { backToMenu = true; break; } 
                            if (post == "2")
                            {
                                exitProgram = true; backToMenu = true; break;
                            }
                            Pause();
                            break;
                        case "2":
                            Console.Write("Fecha inicio (yyyy-MM-dd): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out var f3)) { Console.WriteLine("Fecha inválida"); Pause(); break; }
                            Console.Write("Noches: "); if (!int.TryParse(Console.ReadLine(), out var n3)) { Console.WriteLine("Noches inválidas"); Pause(); break; }
                            bool anyReserved = false;
                            for (int i = 0; i < n3; i++)
                            {
                                var date = f3.Date.AddDays(i);
                                var dia = habitacion.ReservaCalendario.Dias.FirstOrDefault(x => x.Fecha == date);
                                if (dia != null && dia.EstadoActual == EstadoDia.Reservado) { anyReserved = true; break; }
                            }
                            if (!anyReserved) { Console.WriteLine("No hay reservas en esas fechas para liberar."); Pause(); break; }
                            habitacion.ReservaCalendario.Liberar(f3, n3);
                            var startRange = f3.Date;
                            var endRange = f3.Date.AddDays(n3 - 1);
                            var removed = carrito.RemoveAll(x => ReferenceEquals(x.room, habitacion) && x.reserva != null && x.reserva.Start.Date >= startRange && x.reserva.Start.Date <= endRange);
                            Console.WriteLine("Fechas liberadas.");
                            if (removed > 0) Console.WriteLine($"Se eliminaron {removed} reserva(s) del carrito porque fueron liberadas.");
                            Pause();
                            break;
                        case "3":
                            backToMenu = true; break;
                        case "4":
                            var already = carrito.Any(x => ReferenceEquals(x.room, habitacion));
                            if (!already)
                            {
                                var firstRes = habitacion.ReservaCalendario.Dias.FirstOrDefault(d => d.InfoReserva != null)?.InfoReserva;
                                if (firstRes == null) { Console.WriteLine("No hay reservas para esta habitación."); Pause(); break; }
                                carrito.Add((habitacion, firstRes));
                            }
                            Console.WriteLine("Procediendo al pago..."); Pause();
                            exitProgram = true; backToMenu = true;
                            break;
                        case "5":
                            backToMenu = true; break;
                        default:
                            break;
                    }

                    if (exitProgram) break;
                }
                if (exitProgram) break;
            }

            Console.Clear();
            Console.WriteLine("RESUMEN FINAL\n");
            double total = 0;
        
            int width = 60;
            try { width = Math.Min(Console.WindowWidth, 80); } catch { }

            void WriteCenter(string s)
            {
                if (s.Length >= width) Console.WriteLine(s);
                else Console.WriteLine(new string(' ', (width - s.Length) / 2) + s);
            }

            WriteCenter("========================================");
            WriteCenter("RESUMEN DE COMPRA");
            WriteCenter("========================================\n");

            foreach (var item in carrito)
            {
                WriteCenter(item.room.Nombre);
                WriteCenter(new string('-', Math.Min(item.room.Nombre.Length, width - 10)));
                Console.WriteLine(item.room.Descripcion);
                Console.WriteLine("Reserva: " + (item.reserva?.ToString() ?? "sin datos"));

                double precioBase = item.room.Precio;
                double descuento = item.reserva?.DiscountPercent ?? 0.0;
                string motivo = item.reserva?.DiscountReason ?? string.Empty;
                double precioFinal = Math.Round(precioBase * (1.0 - descuento / 100.0), 2);

                Console.WriteLine($"Precio base: {precioBase,10:C2}");
                if (descuento > 0) Console.WriteLine($"Descuento ({motivo}): -{descuento}% -> {precioFinal,10:C2}");
                else Console.WriteLine($"Precio final: {precioFinal,10:C2}");

                bool tieneDesayunoDecorador = item.room is DesayunoDecorator;
                if (precioFinal > 1200 && !tieneDesayunoDecorador)
                {
                    Console.WriteLine("--> Tienes desayuno gratis\n");
                }
                else
                {
                    Console.WriteLine();
                }

                total += precioFinal;
            }

            WriteCenter("----------------------------------------");
            WriteCenter($"TOTAL: {total,10:C2}");
            WriteCenter("----------------------------------------\n");
            WriteCenter("Gracias por usar el sistema.");
            Console.WriteLine($"TOTAL: ${total:F2}");
            Console.WriteLine("Gracias por usar el sistema.");
        }

        static ReservaInfo? PedirDatosReserva(bool checkEmailOptional = false, bool minimalMode = false)
        {
            var info = new ReservaInfo();
            if (minimalMode)
            {
                Console.Write("Nombre: "); info.Nombre = Console.ReadLine() ?? string.Empty;
                Console.Write("Fecha inicio (yyyy-MM-dd): "); if (!DateTime.TryParse(Console.ReadLine(), out var start2)) return null; info.Start = start2;
                info.Telefono = "n/a";
                info.Email = "noreply@local";
                info.Nights = 1;
                return info;
            }

            Console.Write("Nombre: "); info.Nombre = Console.ReadLine() ?? string.Empty;
            Console.Write("Teléfono: "); info.Telefono = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: "); info.Email = Console.ReadLine() ?? string.Empty;
            Console.Write("Fecha inicio (yyyy-MM-dd): "); if (!DateTime.TryParse(Console.ReadLine(), out var start)) return null; info.Start = start;
            Console.Write("Noches: "); if (!int.TryParse(Console.ReadLine(), out var n)) return null; info.Nights = n;
            if (!info.EsValida(out var errores)) { Console.WriteLine("Errores:\n" + errores); return null; }
            return info;
        }

        static void Pause() { Console.WriteLine("Pulsa una tecla para continuar..."); Console.ReadKey(); }
    }
}
