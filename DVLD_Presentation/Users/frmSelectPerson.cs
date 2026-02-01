using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Users
{
    public partial class frmSelectPerson : Form
    {
        public frmSelectPerson()
        {
            InitializeComponent();
        }

        public delegate void SendDataUserById(int personId);
        public event SendDataUserById dataUserById;

        public delegate void SendDataUserByNationalNo(string national);
        public event SendDataUserByNationalNo dataUserByNational;

        private void btnClose_Click(object sender, EventArgs e)
        {
            string txtFilter = ctrlFilterPersonInfo1.tbFilter.Text;
            if(int.TryParse(txtFilter, out int personId))
                dataUserById?.Invoke(personId);
            else
                dataUserByNational?.Invoke(txtFilter);
            this.Close();
        }
    }
}
