using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio1;
using negocio1;
using System.Configuration;
using System.Net.NetworkInformation;

namespace winform_app
{
    public partial class frmAlta : Form
    {
        private Disco disco = null;

        private OpenFileDialog imagenLocal = null;

        private bool cambioDeUrlImagen = false;
        
        public frmAlta()
        {
            InitializeComponent();
        }

        //Constructor para modificar Disco
        public frmAlta(Disco disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificando disco: " + disco.Titulo; 
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
    
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {
                if(disco == null)
                    disco = new Disco();  

                disco.Titulo = txtTitulo.Text; 
                disco.FechaLanzamiento = dtpFechaLanzamiento.Value;
                disco.CantidadCanciones = int.Parse(txtCantidadCanciones.Text);
                disco.Genero = (Estilo)cboGenero.SelectedItem;
                disco.Formato = (TipoEdicion)cboFormato.SelectedItem;

                if(disco.Id == 0)
                {
                    setearUrlImagen(disco);
                    negocio.agregar(disco);
                    MessageBox.Show("Agregado exitosamente");
                }
                else
                {
                    string imagenPrevia = imagenLocalPath();
                    /*if (!string.IsNullOrEmpty(imagenPrevia))
                    {
                        try
                        {
                            if(cambioDeUrlImagen)
                                File.Delete(imagenPrevia);
                        }
                        catch (IOException ex)
                        {
                            // Manejar la excepción de E/S
                            MessageBox.Show("Error al intentar eliminar la imagen previa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }*/
                    setearUrlImagen(disco);
                    negocio.modificar(disco);
                    MessageBox.Show("Disco modificado exitosamente");
                }
                               
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void frmAlta_Load(object sender, EventArgs e)
        {
            EstiloNegocio estiloNegocio = new EstiloNegocio();
            TipoEdicionNegocio tipoEdicionNegocio  = new  TipoEdicionNegocio();

            try
            {
                cboGenero.DataSource = estiloNegocio.listar();
                cboGenero.ValueMember = "Id"; // la clave (para luego poder preseleccionar al modificar)
                cboGenero.DisplayMember = "Descripcion"; // el valor
                cboFormato.DataSource = tipoEdicionNegocio.listar();
                cboFormato.ValueMember = "Id";
                cboFormato.DisplayMember = "Descripcion";

                if (disco != null )
                {
                    txtTitulo.Text = disco.Titulo;
                    dtpFechaLanzamiento.Value = disco.FechaLanzamiento;
                    txtCantidadCanciones.Text = disco.CantidadCanciones.ToString();
                    txtUrlImagenTapa.Text = disco.UrlImagenTapa;
                    cargarImagenTapa(disco.UrlImagenTapa);
                    //Precargo los desplegables utilizando el id del valueMember:
                    cboGenero.SelectedValue = disco.Genero.Id;
                    cboFormato.SelectedValue = disco.Formato.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUrlImagenTapa_Leave(object sender, EventArgs e)
        {
            cargarImagenTapa(txtUrlImagenTapa.Text);
        }

        private void cargarImagenTapa(string imagen)
        {
            try
            {
                pbxImagenTapa.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxImagenTapa.Load("C:\\discos-app\\12in-Vinyl-LP-Record-Angle.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e) // Incorporar imagen local
        {
            imagenLocal = new OpenFileDialog();
            imagenLocal.Filter = "jpg|*.jpg; |png|*.png";
            if(imagenLocal.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagenTapa.Text = imagenLocal.FileName;
                cargarImagenTapa(txtUrlImagenTapa.Text);
            }
            else
            {
                cambioDeUrlImagen = false;
            }
        }

        private string guardarImagenLocal() //Devuelve el path de la imagen guardada localmente
        {
            DateTime fechaHoraActual = DateTime.Now;
            string fechaHoraFormateada = fechaHoraActual.ToString("yyyyMMdd_HHmmss");
            string rutaDestino = ConfigurationManager.AppSettings["images-folder"] + $"{fechaHoraFormateada}_{imagenLocal.SafeFileName}";
            File.Copy(imagenLocal.FileName, rutaDestino);

            return rutaDestino;
        }

        private void setearUrlImagen(Disco disco)
        {
            if (imagenLocal != null && !(txtUrlImagenTapa.Text.ToUpper().Contains("HTTP")))
            {
                disco.UrlImagenTapa = guardarImagenLocal();
            }
            else
            {
                disco.UrlImagenTapa = txtUrlImagenTapa.Text;
            }
        }

        private string imagenLocalPath()
        {
            if(disco != null && File.Exists(disco.UrlImagenTapa))
            {
                return disco.UrlImagenTapa;
            }
            else
            {
                return "";
            }
        }

        private void txtUrlImagenTapa_TextChanged(object sender, EventArgs e)
        {
            cambioDeUrlImagen = true;
        }
    }
}
