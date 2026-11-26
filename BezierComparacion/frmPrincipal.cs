namespace BezierComparacion
{
    public partial class frmPrincipal : Form
    {
        private readonly List<Punto2D> puntosControl = new();

        private List<Punto2D> curvaBernstein = new();
        private PointF[] trazoCasteljau = Array.Empty<PointF>();
        private IReadOnlyList<Punto2D[]> niveles = Array.Empty<Punto2D[]>();

        public frmPrincipal()
        {
            InitializeComponent();
            btnAgregar.Click += AgregarPunto;
            btnModificar.Click += ModificarPunto;
            btnEliminar.Click += EliminarPunto;
            btnLimpiar.Click += Limpiar;
            lstPuntos.SelectedIndexChanged += SeleccionarPunto;
            btnGenerar.Click += GenerarCurva;
            nudMuestras.ValueChanged += CambiarMuestras;
            pnlDibujo.Paint += Dibujar;
            chkInterpolaciones.CheckedChanged += ActualizarInterpolaciones;
            nudParametro.ValueChanged += ActualizarInterpolaciones;
        }

        private void AgregarPunto(object? sender, EventArgs e)
        {
            if (!CoordenadasValidas())
                return;

            puntosControl.Add(new Punto2D((double)nudX.Value, (double)nudY.Value));
            ActualizarLista(puntosControl.Count - 1);
        }

        private void ModificarPunto(object? sender, EventArgs e)
        {
            int indice = lstPuntos.SelectedIndex;
            if (indice < 0)
            {
                MessageBox.Show(this, "Seleccione el punto que desea modificar.");
                return;
            }

            if (!CoordenadasValidas())
                return;

            puntosControl[indice] = new Punto2D((double)nudX.Value, (double)nudY.Value);
            ActualizarLista(indice);
        }

        private void EliminarPunto(object? sender, EventArgs e)
        {
            int indice = lstPuntos.SelectedIndex;
            if (indice < 0)
            {
                MessageBox.Show(this, "Seleccione el punto que desea eliminar.");
                return;
            }

            puntosControl.RemoveAt(indice);
            ActualizarLista(Math.Min(indice, puntosControl.Count - 1));
        }

        private void Limpiar(object? sender, EventArgs e)
        {
            puntosControl.Clear();
            nudX.Value = 0;
            nudY.Value = 0;
            ActualizarLista(-1);
        }

        private void SeleccionarPunto(object? sender, EventArgs e)
        {
            int indice = lstPuntos.SelectedIndex;
            if (indice < 0)
                return;

            nudX.Value = (decimal)puntosControl[indice].X;
            nudY.Value = (decimal)puntosControl[indice].Y;
        }

        private bool CoordenadasValidas()
        {
            if (nudX.Value >= 0 && nudX.Value < pnlDibujo.ClientSize.Width &&
                nudY.Value >= 0 && nudY.Value < pnlDibujo.ClientSize.Height)
                return true;

            MessageBox.Show(this, "Ingrese coordenadas dentro del area de dibujo.");
            return false;
        }

        private void ActualizarLista(int seleccion)
        {
            lstPuntos.Items.Clear();
            for (int i = 0; i < puntosControl.Count; i++)
            {
                Punto2D punto = puntosControl[i];
                lstPuntos.Items.Add($"P{i} ({punto.X:0.##}; {punto.Y:0.##})");
            }

            lstPuntos.SelectedIndex = seleccion;
            lblCantidad.Text = $"Cantidad: {puntosControl.Count}";
            lblGrado.Text = puntosControl.Count == 0 ? "Grado: -" : $"Grado: {puntosControl.Count - 1}";
            curvaBernstein.Clear();
            trazoCasteljau = Array.Empty<PointF>();
            ActualizarInterpolaciones(this, EventArgs.Empty);
        }

        private void CambiarMuestras(object? sender, EventArgs e)
        {
            curvaBernstein.Clear();
            trazoCasteljau = Array.Empty<PointF>();
            pnlDibujo.Invalidate();
        }

        private void GenerarCurva(object? sender, EventArgs e)
        {
            try
            {
                int muestras = (int)nudMuestras.Value;
                var bernstein = Bernstein.GenerarCurva(puntosControl, muestras);
                var casteljau = DeCasteljau.GenerarCurva(puntosControl, muestras);
                trazoCasteljau = casteljau.ConvertAll(punto => new PointF((float)punto.X, (float)punto.Y)).ToArray();
                curvaBernstein = bernstein;
                pnlDibujo.Invalidate();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(this, error.Message);
            }
        }

        private void ActualizarInterpolaciones(object? sender, EventArgs e)
        {
            nudParametro.Enabled = chkInterpolaciones.Checked;
            niveles = chkInterpolaciones.Checked && puntosControl.Count >= 2
                ? DeCasteljau.ObtenerNiveles(puntosControl, (double)nudParametro.Value)
                : Array.Empty<Punto2D[]>();
            lblNiveles.Visible = niveles.Count > 0;
            pnlDibujo.Invalidate();
        }

        private static void DibujarSegmentos(Graphics dibujo, Pen lapiz, IReadOnlyList<Punto2D> puntos)
        {
            for (int i = 1; i < puntos.Count; i++)
            {
                Punto2D anterior = puntos[i - 1];
                Punto2D actual = puntos[i];
                dibujo.DrawLine(lapiz, (float)anterior.X, (float)anterior.Y,
                    (float)actual.X, (float)actual.Y);
            }
        }

        private void Dibujar(object? sender, PaintEventArgs e)
        {
            Graphics dibujo = e.Graphics;
            dibujo.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DibujarSegmentos(dibujo, Pens.Gray, puntosControl);

            using Pen lapizBernstein = new(Color.RoyalBlue, 4);
            using Pen lapizCasteljau = new(Color.DarkOrange, 2)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            DibujarSegmentos(dibujo, lapizBernstein, curvaBernstein);
            // Una sola polilínea conserva el patrón de guiones entre las muestras.
            if (trazoCasteljau.Length >= 2)
                dibujo.DrawLines(lapizCasteljau, trazoCasteljau);

            for (int i = 1; i < niveles.Count; i++)
            {
                bool ultimo = i == niveles.Count - 1;
                Brush color = ultimo ? Brushes.DarkRed : i % 2 == 1 ? Brushes.DarkViolet : Brushes.Teal;
                using Pen lapizNivel = new(color);
                DibujarSegmentos(dibujo, lapizNivel, niveles[i]);
                int radio = ultimo ? 6 : 3;
                foreach (Punto2D punto in niveles[i])
                    dibujo.FillEllipse(color, (float)punto.X - radio, (float)punto.Y - radio, 2 * radio, 2 * radio);
            }

            for (int i = 0; i < puntosControl.Count; i++)
            {
                Punto2D punto = puntosControl[i];
                float x = (float)punto.X;
                float y = (float)punto.Y;
                dibujo.FillEllipse(Brushes.Black, x - 3, y - 3, 6, 6);

                string etiqueta = $"P{i}";
                SizeF tamano = dibujo.MeasureString(etiqueta, pnlDibujo.Font);
                float etiquetaX = Math.Max(0, Math.Min(x + 5, pnlDibujo.ClientSize.Width - tamano.Width));
                float etiquetaY = Math.Max(0, Math.Min(y + 5, pnlDibujo.ClientSize.Height - tamano.Height));
                dibujo.DrawString(etiqueta, pnlDibujo.Font, Brushes.Black, etiquetaX, etiquetaY);
            }
        }
    }
}
