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
using System.IO;
using System.Xml;
//using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace RiznykApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    [Serializable]
    class Folder
    {
        public Folder[] InternalFolders { get; set; }
        public string[] NamesOfFiles { get; set; }
        public string Name { get; set; }
        public Folder(Folder[] InternalFolders1, string[] NamesOfFiles1, string Name1)
        {
            InternalFolders = InternalFolders1;
            NamesOfFiles = NamesOfFiles1;
            Name = Name1;
        }
    }
   
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string dirPath;
            using (FolderBrowserDialog FolderDialog = new FolderBrowserDialog())
            {
                FolderDialog.ShowDialog();
                dirPath = FolderDialog.SelectedPath;

                BinaryFormatter form = new BinaryFormatter();

                using (FileStream fs = new FileStream(System.IO.Directory.GetCurrentDirectory() + @"\serialized.dat", FileMode.Create))
                {
                    form.Serialize(fs, FoldersGetCol(dirPath).ToArray());
                }
                System.Windows.MessageBox.Show("object was serialized");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                openFileDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
                openFileDialog.ShowDialog();
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    BinaryFormatter form = new BinaryFormatter();
                    IEnumerable<Folder> newDir = (IEnumerable<Folder>)form.Deserialize(fs);
                }
                System.Windows.MessageBox.Show("file was deserialized");
            }
        }

        private IEnumerable<Folder> FoldersGetCol(string path)
        {
            foreach (var temp in System.IO.Directory.GetDirectories(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(temp);
                Folder fold = new Folder(FoldersGetCol(temp).ToArray(), System.IO.Directory.GetFiles(temp), dirInfo.Name);
                yield return fold;
            }
        }
    }
}
