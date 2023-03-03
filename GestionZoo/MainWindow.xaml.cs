using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Media.Animation;

namespace GestionZoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection sqlConnection;


        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["GestionZoo.Properties.Settings.EscDirectaDBConnectionString"].ConnectionString;
            
            sqlConnection = new SqlConnection(connectionString);

            MuestraZoo();
            MuestraAnimales();
        }

        private void MuestraZoo()
        {
            try 
            { 
                string consulta = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);

                using (sqlDataAdapter) 
                {
                    DataTable zooTable = new DataTable(); 
                    sqlDataAdapter.Fill(zooTable);

                    ListaZoos.DisplayMemberPath = "Ubicacion";
                    ListaZoos.SelectedValuePath = "Id";
                    ListaZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.ToString()); 
            } 
        }
        private void MuestraAnimales()
        {
            try
            {
                string consulta = "select * from Animal";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTabla = new DataTable();
                    sqlDataAdapter.Fill(animalTabla);

                    ListaAnimales.DisplayMemberPath = "Nombre";
                    ListaAnimales.SelectedValuePath = "Id";
                    ListaAnimales.ItemsSource = animalTabla.DefaultView;
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString()); 
            }
        }
        private void MuestraAnimalesAsociados()
        {

            try
            {
                string consulta = "select * from Animal a Inner Join AnimalZoo az on a.Id = az.AnimalId where az.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);

                    DataTable AnimalTabla = new DataTable();
                    sqlDataAdapter.Fill(AnimalTabla);

                    ListaAnimalesAsociados.DisplayMemberPath = "Nombre";
                    ListaAnimalesAsociados.SelectedValuePath = "Id";
                    ListaAnimalesAsociados.ItemsSource = AnimalTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }


        }


        private void AgregarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Zoo values (@Ubicacion)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraZoo();
            }
        }
        private void AgregarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into AnimalZoo values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraAnimalesAsociados();
            }
        }
        private void AgregarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Animal values (@Nombre)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraAnimales();
            }
        }


        private void EliminarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Animal where id = (@AnimalId,@ZooId)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraAnimales();
            }
        }
        private void EliminarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Zoo where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraZoo();
            }    
        }
        private void EliminarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("perdon, me cuesta hacer la consulta <3");
        }


        private void MuestraZooElegidoTextBox()
        {
            try
            {
                string consulta = "select Ubicacion from Zoo where Id=@ZooId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);

                    DataTable ZooDataTabla = new DataTable();
                    sqlDataAdapter.Fill(ZooDataTabla);

                    miTextBox.Text = ZooDataTabla.Rows[0]["Ubicacion"].ToString();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }
        private void MuestraAnimalElegidoTextBox()
        {
            try
            {
                string consulta = "select Nombre from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);

                    DataTable AnimalDataTabla = new DataTable();
                    sqlDataAdapter.Fill(AnimalDataTabla);

                    miTextBox.Text = AnimalDataTabla.Rows[0]["Nombre"].ToString();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }


        private void ListaAnimales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraAnimalElegidoTextBox();
        }
        private void ListaZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraAnimalesAsociados();
            MuestraZooElegidoTextBox();
        }


        private void ActualizarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Update Zoo Set Ubicacion = @Ubicacion where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraZoo();
            }
        }
        private void ActualizarAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Update Animal Set Nombre = @Nombre where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraAnimales();
            }
        }
    }
}
