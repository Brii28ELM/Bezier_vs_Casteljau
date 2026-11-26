namespace BezierComparacion
{
    public partial class frmPrincipal : Form
    {
        private readonly List<Punto2D> puntosControl = new();

        private List<Punto2D> curva = new();

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
            curva.Clear();
            pnlDibujo.Invalidate();
        }

        private void CambiarMuestras(object? sender, EventArgs e)
        {
            curva.Clear();
            pnlDibujo.Invalidate();
        }

        private void GenerarCurva(object? sender, EventArgs e)
        {
            try
            {
                curva = Bernstein.GenerarCurva(puntosControl, (int)nudMuestras.Value);
                pnlDibujo.Invalidate();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(this, error.Message);
            }
        }

        private void Dibujar(object? sender, PaintEventArgs e)
        {
            Graphics dibujo = e.Graphics;
            for (int i = 1; i < puntosControl.Count; i++)
            {
                Punto2D anterior = puntosControl[i - 1];
                Punto2D actual = puntosControl[i];
                dibujo.DrawLine(Pens.Gray, (float)anterior.X, (float)anterior.Y,
                    (float)actual.X, (float)actual.Y);
            }

            for (int i = 1; i < curva.Count; i++)
            {
                Punto2D anterior = curva[i - 1];
                Punto2D actual = curva[i];
                dibujo.DrawLine(Pens.Blue, (float)anterior.X, (float)anterior.Y,
                    (float)actual.X, (float)actual.Y);
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
