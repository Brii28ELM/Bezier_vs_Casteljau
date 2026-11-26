using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BezierComparacion
{
    public sealed class ResultadoComparacion
    {
        public int Repeticiones { get; set; }
        public int Muestras { get; set; }
        public double TiempoBernsteinMs { get; set; }
        public double TiempoCasteljauMs { get; set; }
        public double DiferenciaMaxima { get; set; }
        public double DiferenciaPromedio { get; set; }
    }

    public static class ComparacionCurvas
    {
        public const int RepeticionesMinimas = 10;

        public static ResultadoComparacion Ejecutar(
            List<Punto2D> puntosControl,
            int cantidadMuestras,
            int repeticiones = RepeticionesMinimas)
        {
            if (puntosControl == null || puntosControl.Count < 2)
                throw new ArgumentException("Se requieren al menos dos puntos de control.", nameof(puntosControl));
            if (cantidadMuestras < 2)
                throw new ArgumentOutOfRangeException(nameof(cantidadMuestras), "Se requieren al menos dos muestras.");
            if (repeticiones < RepeticionesMinimas)
                throw new ArgumentOutOfRangeException(nameof(repeticiones), "Cada prueba debe ejecutarse al menos 10 veces.");

            // Calentamiento: evita que la compilacion JIT afecte la primera medicion.
            Bernstein.GenerarCurva(puntosControl, cantidadMuestras);
            DeCasteljau.GenerarCurva(puntosControl, cantidadMuestras);

            Stopwatch relojBernstein = Stopwatch.StartNew();
            List<Punto2D> curvaBernstein = null!;
            for (int i = 0; i < repeticiones; i++)
                curvaBernstein = Bernstein.GenerarCurva(puntosControl, cantidadMuestras);
            relojBernstein.Stop();

            Stopwatch relojCasteljau = Stopwatch.StartNew();
            List<Punto2D> curvaCasteljau = null!;
            for (int i = 0; i < repeticiones; i++)
                curvaCasteljau = DeCasteljau.GenerarCurva(puntosControl, cantidadMuestras);
            relojCasteljau.Stop();

            double suma = 0;
            double maximo = 0;
            for (int i = 0; i < cantidadMuestras; i++)
            {
                double diferencia = Math.Sqrt(
                    Math.Pow(curvaBernstein[i].X - curvaCasteljau[i].X, 2) +
                    Math.Pow(curvaBernstein[i].Y - curvaCasteljau[i].Y, 2));

                suma += diferencia;
                if (diferencia > maximo)
                    maximo = diferencia;
            }

            return new ResultadoComparacion
            {
                Repeticiones = repeticiones,
                Muestras = cantidadMuestras,
                TiempoBernsteinMs = relojBernstein.Elapsed.TotalMilliseconds,
                TiempoCasteljauMs = relojCasteljau.Elapsed.TotalMilliseconds,
                DiferenciaMaxima = maximo,
                DiferenciaPromedio = suma / cantidadMuestras
            };
        }
    }
}
