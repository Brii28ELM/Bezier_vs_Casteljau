namespace BezierComparacion
{
    public static class Bernstein
    {
        public static Punto2D EvaluarPunto(List<Punto2D> puntosControl, double t)
        {
            if (puntosControl.Count == 0)
                throw new ArgumentException("Se requiere al menos un punto de control.", nameof(puntosControl));
            if (!(t >= 0 && t <= 1))
                throw new ArgumentOutOfRangeException(nameof(t), "t debe estar entre 0 y 1.");

            return EvaluarPunto(puntosControl, t, CalcularCoeficientes(puntosControl.Count - 1));
        }

        public static List<Punto2D> GenerarCurva(List<Punto2D> puntosControl, int cantidadMuestras)
        {
            if (puntosControl.Count < 2)
                throw new ArgumentException("Se requieren al menos dos puntos de control.", nameof(puntosControl));
            if (cantidadMuestras < 2)
                throw new ArgumentOutOfRangeException(nameof(cantidadMuestras), "Se requieren al menos dos muestras.");

            double[] coeficientes = CalcularCoeficientes(puntosControl.Count - 1);
            List<Punto2D> curva = new(cantidadMuestras);
            for (int i = 0; i < cantidadMuestras; i++)
            {
                double t = (double)i / (cantidadMuestras - 1);
                curva.Add(EvaluarPunto(puntosControl, t, coeficientes));
            }

            return curva;
        }

        private static double[] CalcularCoeficientes(int grado)
        {
            double[] coeficientes = new double[grado + 1];
            coeficientes[0] = 1;
            for (int i = 1; i <= grado; i++)
            {
                coeficientes[i] = coeficientes[i - 1] * ((double)(grado - i + 1) / i);
                if (double.IsInfinity(coeficientes[i]))
                    throw new ArgumentException("El grado es demasiado alto para la evaluacion directa.");
            }

            return coeficientes;
        }

        private static Punto2D EvaluarPunto(List<Punto2D> puntosControl, double t, double[] coeficientes)
        {
            int grado = puntosControl.Count - 1;
            if (t == 0)
                return new Punto2D(puntosControl[0].X, puntosControl[0].Y);
            if (t == 1)
                return new Punto2D(puntosControl[grado].X, puntosControl[grado].Y);

            double x = 0;
            double y = 0;
            for (int i = 0; i <= grado; i++)
            {
                double peso = coeficientes[i] * Math.Pow(t, i) * Math.Pow(1 - t, grado - i);
                x += peso * puntosControl[i].X;
                y += peso * puntosControl[i].Y;
            }

            return new Punto2D(x, y);
        }
    }
}
