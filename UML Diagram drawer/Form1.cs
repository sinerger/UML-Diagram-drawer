﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using UML_Diagram_drawer.Forms;
using UML_Diagram_drawer.Arrows;
using Form = System.Windows.Forms.Form;
using UML_Diagram_drawer.Factory;
using UML_Diagram_drawer.MouseHandlers;

namespace UML_Diagram_drawer
{
    public partial class FormMain : Form
    {
        private IFormsFactory _formFactory;
        private MainData _mainData;

        public Pen pen = new Pen(Brushes.Black, 3);

        public FormMain()
        {
            InitializeComponent();
        }

        #region Json
        public string JsonSerialize(TypeOfData type)
        {
            if (type == TypeOfData.Forms)
            {
                string fileDataForms = JsonConvert.SerializeObject(_mainData.FormsList, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                return fileDataForms;
            }
            else if (type == TypeOfData.Arrows)
            {
                string fileDataArrows = JsonConvert.SerializeObject(_mainData.ArrowsList, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                return fileDataArrows;
            }
            throw new Exception();
        }

        public void JsonDeserialize(string[] fileData)
        {
            if (fileData[0] != String.Empty)
            {
                _mainData.FormsList = JsonConvert.DeserializeObject<List<AbstractForm>>(fileData[0],
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            else
            {
                _mainData.FormsList = new List<AbstractForm>();
            }

            if (fileData[1] != null)
            {

                _mainData.ArrowsList = JsonConvert.DeserializeObject<List<AbstactArrow>>(fileData[1],
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            else
            {
                _mainData.ArrowsList = new List<AbstactArrow>();
            }
        }

        #endregion

        private void FormMain_Load(object sender, EventArgs e)
        {
            _formFactory = Default.Factory.Form;
            _mainData = MainData.GetMainData();
            _mainData.PictureBoxMain = pictureBoxMain;
            _mainData.FormsList = new List<AbstractForm>();
            _mainData.ArrowsList = new List<AbstactArrow>();
        }

        #region Tool Strip

        #region CreateArrows
        private void toolStripButtonArrowSuccession_Click(object sender, EventArgs e)
        {
            _mainData.CurrentArrow = new ArrowSuccession();
            _mainData.ArrowsList.Add(_mainData.CurrentArrow);
            _mainData.IMouseHandler = new DrawArrowMouseHandler();
        }

        private void toolStripButtonArrowRealization_Click(object sender, EventArgs e)
        {
            _mainData.CurrentArrow = new ArrowRealization();
            _mainData.ArrowsList.Add(_mainData.CurrentArrow);
            _mainData.IMouseHandler = new DrawArrowMouseHandler();
        }

        private void toolStripButtonArrowAggregation_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonArrowAggregationAndAssociation_Click(object sender, EventArgs e)
        {
            _mainData.CurrentArrow = new ArrowAggregation();
            _mainData.ArrowsList.Add(_mainData.CurrentArrow);
            _mainData.IMouseHandler = new DrawArrowMouseHandler();
        }

        private void toolStripButtonArrowComposition_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonArrowCompositionAndAssociation_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonArrowAssociation_Click(object sender, EventArgs e)
        {
            _mainData.CurrentArrow = new ArrowAssociation();
            _mainData.ArrowsList.Add(_mainData.CurrentArrow);
            _mainData.IMouseHandler = new DrawArrowMouseHandler();
        }
        #endregion

        #region CreateForm
        private void toolStripButtonCreateClassForm_Click(object sender, EventArgs e)
        {
            _formFactory = new ClassFormFactory();
            _mainData.CurrentFormUML = _formFactory.GetForm();
            _mainData.FormsList.Add(_mainData.CurrentFormUML);
            _mainData.IMouseHandler = new DrawFromMouseHandler();
        }
        #endregion


        private void toolStripButtonEditObject_Click(object sender, EventArgs e)
        {
            FormEditor form = new FormEditor(_mainData.SelectForm, pictureBoxMain);
            form.Show();
        }

        private void toolStripButtonSaveFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileDataForms = JsonSerialize(TypeOfData.Forms);
                string fileDataArrows = JsonSerialize(TypeOfData.Arrows);
                SaveAndLoad.SaveFile(saveFileDialog1.FileName, fileDataForms, fileDataArrows);
            }
        }

        private void toolStripButtonOpenFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Переустановить виндовс?", "Во халепа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fileData = SaveAndLoad.OpenFile(openFileDialog1.FileName);
                JsonDeserialize(fileData);
                _mainData.PictureBoxMain.Invalidate();
            }
        }

        #region CopyPaste
        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            copyToStackButton_Click(sender, e);
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            pictureBoxMain.MouseDown += PasteObject_MouseDown;
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            pictureBoxMain.MouseDown += EditObject_MouseDown;
        }

        #endregion

        #endregion

        private void copyToStackButton_Click(object sender, EventArgs e)
        {
            _mainData.FormInBuffer = null;

            foreach (AbstractForm form in _mainData.FormsList)
            {
                if (form.IsSelected)
                {
                    _mainData.FormInBuffer = new UML_Diagram_drawer.Forms.Form(form);
                }
            }
        }

        private void PasteObject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_mainData.FormInBuffer != null)
                {
                    _mainData.FormInBuffer.Location = e.Location;
                    _mainData.FormsList.Add(_mainData.FormInBuffer);
                    _mainData.FormInBuffer = null;
                }
            }
            pictureBoxMain.MouseDown -= PasteObject_MouseDown;
            pictureBoxMain.Invalidate();
        }

        private void EditObject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_mainData.CurrentFormUML != null)
                {
                    _mainData.FormInBuffer = new UML_Diagram_drawer.Forms.Form(_mainData.CurrentFormUML);
                    _mainData.FormsList.Remove(_mainData.CurrentFormUML);
                }
            }

            pictureBoxMain.Invalidate();
        }

        #region PictureBoxEvent
        private void pictureBoxMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mainData.IMouseHandler != null)
            {
                _mainData.IMouseHandler.MouseClick(sender, e);
            }
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mainData.IMouseHandler != null)
            {
                _mainData.IMouseHandler.MouseDown(sender, e);
            }
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_mainData.IMouseHandler != null)
            {
                _mainData.IMouseHandler.MouseUp(sender, e);
            }
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mainData.IMouseHandler != null)
            {
                _mainData.IMouseHandler.MouseMove(sender, e);
            }
        }

        private void PictureBoxMain_Paint(object sender, PaintEventArgs e)
        {
            MainGraphics.Graphics = e.Graphics;

            foreach (var arrow in _mainData.ArrowsList)
            {
                arrow.Draw();
            }

            foreach (AbstractForm form in _mainData.FormsList)
            {
                form.Draw();
            }
        }

        #endregion

        private void toolStripButtonSaveImageFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
                pictureBoxMain.DrawToBitmap(bmp, new Rectangle(0, 0, pictureBoxMain.Width, pictureBoxMain.Height));
                bmp.Save(saveFileDialog2.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void toolStripButtonSelectForm_Click(object sender, EventArgs e)
        {
            _mainData.IMouseHandler = new SelectFormMouseHandler();
        }
        private void flowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            const int step = 50;
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                pictureBoxMain.Height += e.NewValue - e.OldValue + step;
            }
            else
            {
                pictureBoxMain.Width += e.NewValue - e.OldValue + step;
            }
        }
    }
}