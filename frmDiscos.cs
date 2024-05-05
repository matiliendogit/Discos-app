using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio1;
using negocio1;

namespace winform_app
{
    public partial class frmDiscos : Form
    {
        
        private List<Disco> listaDisco;

        public frmDiscos()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Título");
            cboCampo.Items.Add("Cantidad de Canciones");
            cboCampo.Items.Add("Género");
            cboCampo.Items.Add("Formato");
        }

        private void cargar()
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                listaDisco = negocio.listar();
                dgvDiscos.DataSource = listaDisco;
                
                ocultarColumnas();
                cargarImagen(listaDisco[0].UrlImagenTapa);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagenTapa"].Visible = false;
            dgvDiscos.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                picbDiscos.Load(imagen);
            }
            catch (Exception ex)
            {
                picbDiscos.Load("https://wpdirecto.com/wp-content/uploads/2017/08/alt-de-una-imagen.png");
            }
        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiscos.CurrentRow != null)
            {
                Disco seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagenTapa);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAlta alta = new frmAlta();
            alta.ShowDialog();
            cargar(); // Para actualizar el contenido de la datagridview
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            abrirModificar();
        }

        private void dgvDiscos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            abrirModificar();
        }

        private void abrirModificar()
        {
            Disco seleccionado;
            seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;

            frmAlta modificar = new frmAlta(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisica_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true); // Cambia la columna activo a 0;
        }

        private void eliminar(bool logico = false)//Por defecto elimina fisicamente y si paso true es logico
        {
            DiscosNegocio negocio = new DiscosNegocio();
            Disco seleccionado;
            try
            {
                seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
                DialogResult respuesta = MessageBox.Show("De verdad deseas eliminar el disco " + seleccionado.Titulo, "Eliminando disco", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); // devuelve un dialog result que me da como respuesta Yes y No.

                if (respuesta == DialogResult.Yes)
                {
                    if (logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);
                        cargar();
                    }
                    else
                    {
                        negocio.eliminar(seleccionado.Id);
                        cargar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                if (cboCampo.SelectedItem == null || cboCriterio.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, seleccione un campo y un criterio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                if (campo == "Cantidad de Canciones")
                {
                    dgvDiscos.DataSource = negocio.filtrar(campo, criterio, filtro, true);
                }
                else
                {
                    dgvDiscos.DataSource = negocio.filtrar(campo, criterio, filtro, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Disco> listaFiltrada;
            string filtro = txtFiltroRapido.Text;


            if (filtro.Length >= 2)
            {
                listaFiltrada = listaDisco.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper()) || x.Genero.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Formato.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDisco;
            }
            dgvDiscos.DataSource = null; // Primero siempre hay que vaciar
            dgvDiscos.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = cboCampo.SelectedItem.ToString();

            if(campo == "Cantidad de Canciones")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Igual a");
                cboCriterio.Items.Add("Menor a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
