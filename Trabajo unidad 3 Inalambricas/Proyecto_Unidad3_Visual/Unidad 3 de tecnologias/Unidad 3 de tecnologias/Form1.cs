using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Unidad_3_de_tecnologias
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort; // Objeto para manejar la comunicación serial

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializa el puerto serial y configura sus parámetros básicos
            serialPort = new SerialPort();
            serialPort.BaudRate = 9600; // Velocidad de transmisión configurada en 9600 baudios
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); // Asocia un evento para procesar los datos que lleguen

            // Agrega los nombres de los puertos COM detectados al ComboBox
            foreach (string port in SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(port);
            }
        }

        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPorts.SelectedItem != null)
                {
                    serialPort.PortName = comboBoxPorts.SelectedItem.ToString(); // Define el puerto COM seleccionado
                    serialPort.Open(); // Establece la conexión serial
                    MessageBox.Show("Conexión establecida con " + serialPort.PortName);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un puerto COM antes de conectar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error al intentar conectar: " + ex.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // Captura los datos que se reciben desde el puerto serial
            string inData = serialPort.ReadLine(); // Lee una línea completa de datos entrantes

            // Actualiza la interfaz gráfica para mostrar la información recibida
            this.Invoke(new MethodInvoker(delegate
            {
                richTextBoxReceivedData.AppendText(inData + Environment.NewLine + "\n"); // Añade los datos al cuadro de texto

                // Interpreta los datos recibidos como una cadena binaria
                // Por ejemplo: '01000001' corresponde al carácter 'A'
                if (!string.IsNullOrEmpty(inData))
                {
                    try
                    {
                        // Elimina espacios innecesarios y valida la longitud de los datos
                        string trimmedData = inData.Trim();
                        if (trimmedData.Length == 8) // Verifica que los datos sean de 8 bits
                        {
                            int charCode = Convert.ToInt32(trimmedData, 2); // Convierte la cadena binaria en un número entero
                            char letra = (char)charCode; // Convierte el entero en un carácter ASCII
                            richTextBoxFrase.AppendText(letra.ToString()); // Muestra el carácter en el segundo cuadro de texto
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al interpretar los datos: " + ex.Message);
                    }
                }
            }));
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // Asegura que la conexión serial se cierre al cerrar la aplicación
            MessageBox.Show("El programa se está cerrando. \n" + "Puerto " + serialPort.PortName + " desconectado.");
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void btnDesconnected_Click(object sender, EventArgs e)
        {
            // Permite al usuario desconectar el puerto manualmente
            if (serialPort.IsOpen)
            {
                MessageBox.Show("La conexión con el puerto " + serialPort.PortName + " ha sido cerrada.");
                serialPort.Close();
                richTextBoxReceivedData.Text = ""; // Limpia los datos recibidos en el primer cuadro de texto
                richTextBoxFrase.Text = ""; // Limpia los caracteres interpretados en el segundo cuadro de texto
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Confirma que el usuario desea salir y cierra la conexión antes de cerrar el programa
            MessageBox.Show("El programa se está cerrando. \n" + "Puerto " + serialPort.PortName + " desconectado.");

            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            this.Close();
        }
    }
}
