 using DVLD_Presentation.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Controls
{
    public partial class ctrlFilterPersonInfo : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void personSelected(int personId)
        {
            Action<int> Handler = OnPersonSelected;
            if(Handler != null) Handler(personId);
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set
            {
                _ShowAddPerson = value;
                button1.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        public ctrlFilterPersonInfo()
        {
            InitializeComponent();
        }

        public int PersonId
        {
            get
            {
                return ctrlPersonInfo1.PersonId;
            }
        }

        public void FilterFoucs()
        {
            tbFilter.Focus();
        }

        private void _LoadFiltersInBox()
        {
            cbFilter.Items.Add("National No.");
            cbFilter.Items.Add("Person ID");
            cbFilter.SelectedIndex = 0;
        }

        private void ctrlFilterPersonInfo_Load(object sender, EventArgs e)
        {
            _LoadFiltersInBox();
        } 

        public void LoadPersonInfo(int personId)
        {
            tbFilter.Text = personId.ToString();
            cbFilter.SelectedIndex = 1;
            FindNow();
        }

        private void FindNow()
        {
            switch (cbFilter.Text)
            {
                case "Person ID":
                    ctrlPersonInfo1.LoadPersonInfo(int.Parse(tbFilter.Text));
                    break;
                case "National No.":
                    ctrlPersonInfo1.LoadPersonInfo(tbFilter.Text);
                    break;
                default:
                    break;
            }
            if(OnPersonSelected != null && FilterEnabled)
                personSelected(PersonId);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addEditPerson = new frmAddEditPerson();
            addEditPerson.Person += LoadPersonInfo;
            addEditPerson.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FindNow();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnAdd.PerformClick();

            if(cbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
