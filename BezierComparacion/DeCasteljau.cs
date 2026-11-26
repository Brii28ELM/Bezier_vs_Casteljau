namespace BezierComparacion
{
    public static class DeCasteljau
    {
        public static Punto2D EvaluarPunto(IReadOnlyList<Punto2D> puntosControl, double t)
        {
            ValidarEvaluacion(puntosControl, t);
            return Evaluar(puntosControl, t, new (double X, double Y)[puntosControl.Count]);
        }

        public static List<Punto2D> GenerarCurva(IReadOnlyList<Punto2D> puntosControl, int cantidadMuestras)
        {
            if (puntosControl.Count < 2)
                throw new ArgumentException("Se requieren al menos dos puntos de control.", nameof(puntosControl));
            if (cantidadMuestras < 2)
                throw new ArgumentOutOfRangeException(nameof(cantidadMuestras), "Se requieren al menos dos muestras.");

            var trabajo = new (double X, double Y)[puntosControl.Count];
            List<Punto2D> curva = new(cantidadMuestras);
            for (int i = 0; i < cantidadMuestras; i++)
            {
                double t = (double)i / (cantidadMuestras - 1);
                curva.Add(Evaluar(puntosControl, t, trabajo));
            }

            return curva;
        }

        public static IReadOnlyList<Punto2D[]> ObtenerNiveles(IReadOnlyList<Punto2D> puntosControl, double t)
        {
            ValidarEvaluacion(puntosControl, t);
            var trabajo = new (double X, double Y)[puntosControl.Count];
            CopiarPuntos(puntosControl, trabajo);

            // El nivel cero es el polígono de control. El último nivel contiene B(t).
            List<Punto2D[]> niveles = new(puntosControl.Count);
            for (int longitud = trabajo.Length; longitud > 0; longitud--)
            {
                Punto2D[] nivel = new Punto2D[longitud];
                for (int i = 0; i < longitud; i++)
                    nivel[i] = new Punto2D(trabajo[i].X, trabajo[i].Y);

                niveles.Add(nivel);
                InterpolarNivel(trabajo, longitud - 1, t);
            }

            return niveles;
        }

        private static void ValidarEvaluacion(IReadOnlyList<Punto2D> puntosControl, double t)
        {
            if (puntosControl.Count == 0)
                throw new ArgumentException("Se requiere al menos un punto de control.", nameof(puntosControl));
            if (!(t >= 0 && t <= 1))
                throw new ArgumentOutOfRangeException(nameof(t), "t debe estar entre 0 y 1.");
        }

        private static Punto2D Evaluar(IReadOnlyList<Punto2D> puntosControl, double t,
            (double X, double Y)[] trabajo)
        {
            if (t == 0 || t == 1)
            {
                Punto2D extremo = puntosControl[t == 0 ? 0 : puntosControl.Count - 1];
                return new Punto2D(extremo.X, extremo.Y);
            }

            CopiarPuntos(puntosControl, trabajo);
            for (int longitud = trabajo.Length - 1; longitud > 0; longitud--)
                InterpolarNivel(trabajo, longitud, t);

            return new Punto2D(trabajo[0].X, trabajo[0].Y);
        }

        private static void CopiarPuntos(IReadOnlyList<Punto2D> puntosControl, (double X, double Y)[] trabajo)
        {
            for (int i = 0; i < trabajo.Length; i++)
                trabajo[i] = (puntosControl[i].X, puntosControl[i].Y);
        }

        private static void InterpolarNivel((double X, double Y)[] trabajo, int longitud, double t)
        {
            double complemento = 1 - t;
            for (int i = 0; i < longitud; i++)
            {
                var a = trabajo[i];
                var b = trabajo[i + 1];
                trabajo[i] = (complemento * a.X + t * b.X, complemento * a.Y + t * b.Y);
            }
        }
    }
}
