namespace BezierComparacion
{
    partial class frmPrincipal
    {
        private Panel pnlDibujo = null!;
        private Label lblX = null!;
        private NumericUpDown nudX = null!;
        private Label lblY = null!;
        private NumericUpDown nudY = null!;
        private Button btnAgregar = null!;
        private Label lblPuntos = null!;
        private ListBox lstPuntos = null!;
        private Button btnModificar = null!;
        private Button btnEliminar = null!;
        private Label lblCantidad = null!;
        private Label lblGrado = null!;
        private Label lblMuestras = null!;
        private NumericUpDown nudMuestras = null!;
        private Button btnGenerar = null!;
        private Button btnLimpiar = null!;

        private void InitializeComponent()
        {
            pnlDibujo = new Panel();
            lblX = new Label();
            nudX = new NumericUpDown();
            lblY = new Label();
            nudY = new NumericUpDown();
            btnAgregar = new Button();
            lblPuntos = new Label();
            lstPuntos = new ListBox();
            btnModificar = new Button();
            btnEliminar = new Button();
            lblCantidad = new Label();
            lblGrado = new Label();
            lblMuestras = new Label();
            nudMuestras = new NumericUpDown();
            btnGenerar = new Button();
            btnLimpiar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMuestras).BeginInit();
            SuspendLayout();

            pnlDibujo.Name = "pnlDibujo";
            pnlDibujo.Location = new Point(12, 12);
            pnlDibujo.Size = new Size(660, 480);
            pnlDibujo.TabIndex = 0;
            pnlDibujo.BackColor = Color.White;
            pnlDibujo.BorderStyle = BorderStyle.FixedSingle;

            lblX.Name = "lblX";
            lblX.Location = new Point(690, 15);
            lblX.Size = new Size(45, 23);
            lblX.TabIndex = 1;
            lblX.Text = "X";

            nudX.Name = "nudX";
            nudX.Location = new Point(740, 12);
            nudX.Size = new Size(228, 23);
            nudX.TabIndex = 2;
            nudX.DecimalPlaces = 2;
            nudX.Maximum = 657;
            nudX.AccessibleName = "Coordenada X";

            lblY.Name = "lblY";
            lblY.Location = new Point(690, 47);
            lblY.Size = new Size(45, 23);
            lblY.TabIndex = 3;
            lblY.Text = "Y";

            nudY.Name = "nudY";
            nudY.Location = new Point(740, 44);
            nudY.Size = new Size(228, 23);
            nudY.TabIndex = 4;
            nudY.DecimalPlaces = 2;
            nudY.Maximum = 477;
            nudY.AccessibleName = "Coordenada Y";

            btnAgregar.Name = "btnAgregar";
            btnAgregar.Location = new Point(690, 78);
            btnAgregar.Size = new Size(278, 30);
            btnAgregar.TabIndex = 5;
            btnAgregar.Text = "Agregar punto";

            lblPuntos.Name = "lblPuntos";
            lblPuntos.Location = new Point(690, 119);
            lblPuntos.Size = new Size(278, 23);
            lblPuntos.TabIndex = 6;
            lblPuntos.Text = "Puntos de control";

            lstPuntos.Name = "lstPuntos";
            lstPuntos.Location = new Point(690, 143);
            lstPuntos.Size = new Size(278, 124);
            lstPuntos.TabIndex = 7;
            lstPuntos.IntegralHeight = false;
            lstPuntos.AccessibleName = "Puntos de control";

            btnModificar.Name = "btnModificar";
            btnModificar.Location = new Point(690, 277);
            btnModificar.Size = new Size(135, 30);
            btnModificar.TabIndex = 8;
            btnModificar.Text = "Modificar punto";

            btnEliminar.Name = "btnEliminar";
            btnEliminar.Location = new Point(833, 277);
            btnEliminar.Size = new Size(135, 30);
            btnEliminar.TabIndex = 9;
            btnEliminar.Text = "Eliminar punto";

            lblCantidad.Name = "lblCantidad";
            lblCantidad.Location = new Point(690, 318);
            lblCantidad.Size = new Size(135, 23);
            lblCantidad.TabIndex = 10;
            lblCantidad.Text = "Cantidad: 0";

            lblGrado.Name = "lblGrado";
            lblGrado.Location = new Point(833, 318);
            lblGrado.Size = new Size(135, 23);
            lblGrado.TabIndex = 11;
            lblGrado.Text = "Grado: -";

            lblMuestras.Name = "lblMuestras";
            lblMuestras.Location = new Point(690, 354);
            lblMuestras.Size = new Size(95, 23);
            lblMuestras.TabIndex = 12;
            lblMuestras.Text = "Muestras";

            nudMuestras.Name = "nudMuestras";
            nudMuestras.Location = new Point(790, 351);
            nudMuestras.Size = new Size(178, 23);
            nudMuestras.TabIndex = 13;
            nudMuestras.Minimum = 2;
            nudMuestras.Maximum = 10000;
            nudMuestras.Value = 100;
            nudMuestras.AccessibleName = "Cantidad de muestras";

            btnGenerar.Name = "btnGenerar";
            btnGenerar.Location = new Point(690, 389);
            btnGenerar.Size = new Size(278, 30);
            btnGenerar.TabIndex = 14;
            btnGenerar.Text = "Generar curva";

            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Location = new Point(690, 427);
            btnLimpiar.Size = new Size(278, 30);
            btnLimpiar.TabIndex = 15;
            btnLimpiar.Text = "Limpiar";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 504);
            Controls.Add(pnlDibujo);
            Controls.Add(lblX);
            Controls.Add(nudX);
            Controls.Add(lblY);
            Controls.Add(nudY);
            Controls.Add(btnAgregar);
            Controls.Add(lblPuntos);
            Controls.Add(lstPuntos);
            Controls.Add(btnModificar);
            Controls.Add(btnEliminar);
            Controls.Add(lblCantidad);
            Controls.Add(lblGrado);
            Controls.Add(lblMuestras);
            Controls.Add(nudMuestras);
            Controls.Add(btnGenerar);
            Controls.Add(btnLimpiar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmPrincipal";
            Text = "Comparacion de curvas de Bezier";
            ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMuestras).EndInit();
            ResumeLayout(false);
        }
    }
}
