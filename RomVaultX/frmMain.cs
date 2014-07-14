﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RomVaultX
{
    public partial class frmMain : Form
    {

        private bool _updatingGameGrid;

        private Single _scaleFactorX = 1;
        private Single _scaleFactorY = 1;

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            splitContainer1.SplitterDistance = (int)(splitContainer1.SplitterDistance * factor.Width);
            splitContainer2.SplitterDistance = (int)(splitContainer2.SplitterDistance * factor.Width);
            splitContainer2.Panel1MinSize = (int)(splitContainer2.Panel1MinSize * factor.Width);

            splitContainer3.SplitterDistance = (int)(splitContainer3.SplitterDistance * factor.Height);
            splitContainer4.SplitterDistance = (int)(splitContainer4.SplitterDistance * factor.Height);

            _scaleFactorX *= factor.Width;
            _scaleFactorY *= factor.Height;
        }

        public frmMain()
        {
            InitializeComponent();
            addGameGrid();
            DirTree.Setup(DataAccessLayer.ReadTreeFromDB());
        }

        private void btnUpdateDats_Click(object sender, EventArgs e)
        {
            UpdateDats();
            DirTree.Setup(DataAccessLayer.ReadTreeFromDB());
        }

        private void UpdateDats()
        {
            FrmProgressWindow progress = new FrmProgressWindow(this, "Scanning Dats", DatUpdate.UpdateDat);
            progress.ShowDialog(this);
            progress.Dispose();
        }

        private void DirTree_RvSelected(object sender, MouseEventArgs e)
        {
            RvTreeRow tr = (RvTreeRow)sender;
            Debug.WriteLine(tr.dirFullName);
            updateSelectedTreeRow(tr);
        }



        #region DAT dsiplay code
        private void splitContainer3_Panel1_Resize(object sender, EventArgs e)
        {
            gbDatInfo.Width = splitContainer3.Panel1.Width - (gbDatInfo.Left * 2);
        }


        private void gbDatInfo_Resize(object sender, EventArgs e)
        {
            const int leftPos = 89;
            int rightPos = (int)(gbDatInfo.Width / _scaleFactorX) - 15;
            if (rightPos > 600) rightPos = 600;
            int width = rightPos - leftPos;
            int widthB1 = (int)((double)width * 120 / 340);
            int leftB2 = rightPos - widthB1;


            int backD = 97;

            width = (int)(width * _scaleFactorX);
            widthB1 = (int)(widthB1 * _scaleFactorX);
            leftB2 = (int)(leftB2 * _scaleFactorX);
            backD = (int)(backD * _scaleFactorX);


            lblDITName.Width = width;
            lblDITDescription.Width = width;

            lblDITCategory.Width = widthB1;
            lblDITAuthor.Width = widthB1;

            lblDIVersion.Left = leftB2 - backD;
            lblDIDate.Left = leftB2 - backD;

            lblDITVersion.Left = leftB2;
            lblDITVersion.Width = widthB1;
            lblDITDate.Left = leftB2;
            lblDITDate.Width = widthB1;

            lblDITPath.Width = width;

            lblDITRomsGot.Width = widthB1;
            lblDITRomsMissing.Width = widthB1;

            lblDIRomsFixable.Left = leftB2 - backD;
            lblDIRomsUnknown.Left = leftB2 - backD;

            lblDITRomsFixable.Left = leftB2;
            lblDITRomsFixable.Width = widthB1;
            lblDITRomsUnknown.Left = leftB2;
            lblDITRomsUnknown.Width = widthB1;
        }




        private void updateSelectedTreeRow(RvTreeRow tr)
        {
            lblDITName.Text = tr.datName;
            lblDITPath.Text = tr.dirFullName;

            if (tr.DatId != null)
            {
                string Description, Category, Version, Author, Date;
                DataAccessLayer.ReadDatInfo((int)tr.DatId, out Description, out Category, out Version, out Author, out Date);
                lblDITDescription.Text = Description;
                lblDITCategory.Text = Category;
                lblDITVersion.Text = Version;
                lblDITAuthor.Text = Author;
                lblDITDate.Text = Date;
            }
            else
            {
                lblDITDescription.Text = "";
                lblDITCategory.Text = "";
                lblDITVersion.Text = "";
                lblDITAuthor.Text = "";
                lblDITDate.Text = "";
            }

            UpdateGameGrid(tr.DatId);

        }

        #endregion


        #region Game display code

        private Label lblSIName;
        private Label lblSITName;

        private Label lblSIDescription;
        private Label lblSITDescription;

        private Label lblSIManufacturer;
        private Label lblSITManufacturer;

        private Label lblSICloneOf;
        private Label lblSITCloneOf;

        private Label lblSIRomOf;
        private Label lblSITRomOf;

        private Label lblSIYear;
        private Label lblSITYear;

        private Label lblSITotalRoms;
        private Label lblSITTotalRoms;

        //Trurip Extra Data
        private Label lblSIPublisher;
        private Label lblSITPublisher;

        private Label lblSIDeveloper;
        private Label lblSITDeveloper;

        private Label lblSIEdition;
        private Label lblSITEdition;

        private Label lblSIVersion;
        private Label lblSITVersion;

        private Label lblSIType;
        private Label lblSITType;

        private Label lblSIMedia;
        private Label lblSITMedia;

        private Label lblSILanguage;
        private Label lblSITLanguage;

        private Label lblSIPlayers;
        private Label lblSITPlayers;

        private Label lblSIRatings;
        private Label lblSITRatings;

        private Label lblSIGenre;
        private Label lblSITGenre;

        private Label lblSIPeripheral;
        private Label lblSITPeripheral;

        private Label lblSIBarCode;
        private Label lblSITBarCode;

        private Label lblSIMediaCatalogNumber;
        private Label lblSITMediaCatalogNumber;

        private void addGameGrid()
        {
            lblSIName = new Label { Location = SPoint(6, 15), Size = SSize(76, 13), Text = "Name :", TextAlign = ContentAlignment.TopRight };
            lblSITName = new Label { Location = SPoint(84, 14), Size = SSize(320, 17), BorderStyle = BorderStyle.FixedSingle };
            gbSetInfo.Controls.Add(lblSIName);
            gbSetInfo.Controls.Add(lblSITName);

            lblSIDescription = new Label { Location = SPoint(6, 31), Size = SSize(76, 13), Text = "Description :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITDescription = new Label { Location = SPoint(84, 30), Size = SSize(320, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIDescription);
            gbSetInfo.Controls.Add(lblSITDescription);

            lblSIManufacturer = new Label { Location = SPoint(6, 47), Size = SSize(76, 13), Text = "Manufacturer :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITManufacturer = new Label { Location = SPoint(84, 46), Size = SSize(320, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIManufacturer);
            gbSetInfo.Controls.Add(lblSITManufacturer);

            lblSICloneOf = new Label { Location = SPoint(6, 63), Size = SSize(76, 13), Text = "Clone of :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITCloneOf = new Label { Location = SPoint(84, 62), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSICloneOf);
            gbSetInfo.Controls.Add(lblSITCloneOf);

            lblSIYear = new Label { Location = SPoint(206, 63), Size = SSize(76, 13), Text = "Year :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITYear = new Label { Location = SPoint(284, 62), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIYear);
            gbSetInfo.Controls.Add(lblSITYear);


            lblSIRomOf = new Label { Location = SPoint(6, 79), Size = SSize(76, 13), Text = "ROM of :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITRomOf = new Label { Location = SPoint(84, 78), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIRomOf);
            gbSetInfo.Controls.Add(lblSITRomOf);

            lblSITotalRoms = new Label { Location = SPoint(206, 79), Size = SSize(76, 13), Text = "Total ROMs :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITTotalRoms = new Label { Location = SPoint(284, 78), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSITotalRoms);
            gbSetInfo.Controls.Add(lblSITTotalRoms);

            //Trurip

            lblSIPublisher = new Label { Location = SPoint(6, 47), Size = SSize(76, 13), Text = "Publisher :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITPublisher = new Label { Location = SPoint(84, 46), Size = SSize(320, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIPublisher);
            gbSetInfo.Controls.Add(lblSITPublisher);

            lblSIDeveloper = new Label { Location = SPoint(6, 63), Size = SSize(76, 13), Text = "Developer :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITDeveloper = new Label { Location = SPoint(84, 62), Size = SSize(320, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIDeveloper);
            gbSetInfo.Controls.Add(lblSITDeveloper);



            lblSIEdition = new Label { Location = SPoint(6, 79), Size = SSize(76, 13), Text = "Edition :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITEdition = new Label { Location = SPoint(84, 78), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIEdition);
            gbSetInfo.Controls.Add(lblSITEdition);

            lblSIVersion = new Label { Location = SPoint(206, 79), Size = SSize(76, 13), Text = "Version :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITVersion = new Label { Location = SPoint(284, 78), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIVersion);
            gbSetInfo.Controls.Add(lblSITVersion);

            lblSIType = new Label { Location = SPoint(406, 79), Size = SSize(76, 13), Text = "Type :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITType = new Label { Location = SPoint(484, 78), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIType);
            gbSetInfo.Controls.Add(lblSITType);


            lblSIMedia = new Label { Location = SPoint(6, 95), Size = SSize(76, 13), Text = "Media :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITMedia = new Label { Location = SPoint(84, 94), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIMedia);
            gbSetInfo.Controls.Add(lblSITMedia);

            lblSILanguage = new Label { Location = SPoint(206, 95), Size = SSize(76, 13), Text = "Language :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITLanguage = new Label { Location = SPoint(284, 94), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSILanguage);
            gbSetInfo.Controls.Add(lblSITLanguage);

            lblSIPlayers = new Label { Location = SPoint(406, 95), Size = SSize(76, 13), Text = "Players :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITPlayers = new Label { Location = SPoint(484, 94), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIPlayers);
            gbSetInfo.Controls.Add(lblSITPlayers);



            lblSIRatings = new Label { Location = SPoint(6, 111), Size = SSize(76, 13), Text = "Ratings :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITRatings = new Label { Location = SPoint(84, 110), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIRatings);
            gbSetInfo.Controls.Add(lblSITRatings);

            lblSIGenre = new Label { Location = SPoint(206, 111), Size = SSize(76, 13), Text = "Genre :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITGenre = new Label { Location = SPoint(284, 110), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIGenre);
            gbSetInfo.Controls.Add(lblSITGenre);

            lblSIPeripheral = new Label { Location = SPoint(406, 111), Size = SSize(76, 13), Text = "Peripheral :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITPeripheral = new Label { Location = SPoint(484, 110), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIPeripheral);
            gbSetInfo.Controls.Add(lblSITPeripheral);


            lblSIBarCode = new Label { Location = SPoint(6, 127), Size = SSize(76, 13), Text = "Barcode :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITBarCode = new Label { Location = SPoint(84, 126), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIBarCode);
            gbSetInfo.Controls.Add(lblSITBarCode);

            lblSIMediaCatalogNumber = new Label { Location = SPoint(406, 127), Size = SSize(76, 13), Text = "Cat. No. :", TextAlign = ContentAlignment.TopRight, Visible = false };
            lblSITMediaCatalogNumber = new Label { Location = SPoint(484, 126), Size = SSize(120, 17), BorderStyle = BorderStyle.FixedSingle, Visible = false };
            gbSetInfo.Controls.Add(lblSIMediaCatalogNumber);
            gbSetInfo.Controls.Add(lblSITMediaCatalogNumber);

        }
        private Point SPoint(int x, int y)
        {
            return new Point((int)(x * _scaleFactorX), (int)(y * _scaleFactorY));
        }
        private Size SSize(int x, int y)
        {
            return new Size((int)(x * _scaleFactorX), (int)(y * _scaleFactorY));
        }

        private void gbSetInfo_Resize(object sender, EventArgs e)
        {
            int leftPos = 84;
            int rightPos = gbSetInfo.Width - 15;
            if (rightPos > 750) rightPos = 750;
            int width = rightPos - leftPos;

            int widthB1 = (int)((double)width * 120 / 340);
            int leftB2 = leftPos + width - widthB1;

            if (lblSITName == null) return;

            lblSITName.Width = width;
            lblSITDescription.Width = width;
            lblSITManufacturer.Width = width;

            lblSITCloneOf.Width = widthB1;

            lblSIYear.Left = leftB2 - 78;
            lblSITYear.Left = leftB2;
            lblSITYear.Width = widthB1;

            lblSITRomOf.Width = widthB1;

            lblSITotalRoms.Left = leftB2 - 78;
            lblSITTotalRoms.Left = leftB2;
            lblSITTotalRoms.Width = widthB1;

            lblSITPublisher.Width = width;
            lblSITDeveloper.Width = width;

            int width3 = (int)((double)width * 0.24);
            int P2 = (int)((double)width * 0.38);

            int width4 = (int)((double)width * 0.24);

            lblSITEdition.Width = width3;

            lblSIVersion.Left = leftPos + P2 - 78;
            lblSITVersion.Left = leftPos + P2;
            lblSITVersion.Width = width3;

            lblSIType.Left = leftPos + width - width3 - 78;
            lblSITType.Left = leftPos + width - width3;
            lblSITType.Width = width3;


            lblSITMedia.Width = width3;

            lblSILanguage.Left = leftPos + P2 - 78;
            lblSITLanguage.Left = leftPos + P2;
            lblSITLanguage.Width = width3;

            lblSIPlayers.Left = leftPos + width - width3 - 78;
            lblSITPlayers.Left = leftPos + width - width3;
            lblSITPlayers.Width = width3;

            lblSITRatings.Width = width3;

            lblSIGenre.Left = leftPos + P2 - 78;
            lblSITGenre.Left = leftPos + P2;
            lblSITGenre.Width = width3;

            lblSIPeripheral.Left = leftPos + width - width3 - 78;
            lblSITPeripheral.Left = leftPos + width - width3;
            lblSITPeripheral.Width = width3;


            lblSITBarCode.Width = width4;

            lblSIMediaCatalogNumber.Left = leftPos + width - width4 - 78;
            lblSITMediaCatalogNumber.Left = leftPos + width - width4;
            lblSITMediaCatalogNumber.Width = width4;


        }



        private void UpdateGameGrid(int? DatId)
        {
            _updatingGameGrid = true;
            GameGrid.Rows.Clear();
            RomGrid.Rows.Clear();

            if (DatId == null)
                return;

            List<rvGameRow> rows = DataAccessLayer.ReadGames((int)DatId);

            foreach (rvGameRow row in rows)
            {
                GameGrid.Rows.Add();
                int iRow = GameGrid.Rows.Count - 1;

                GameGrid.Rows[iRow].Selected = false;
                GameGrid.Rows[iRow].Tag = row.GameId;
                GameGrid.Rows[iRow].Cells[1].Value = row.Name;
                GameGrid.Rows[iRow].Cells[2].Value = row.Description;
            }
            _updatingGameGrid = false;
            
        }
        private void GameGrid_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedGame();
        }

        private void UpdateSelectedGame()
        {
            if (_updatingGameGrid)
                return;

            if (GameGrid.SelectedRows.Count != 1)
                return;

            int GameId=(int)GameGrid.SelectedRows[0].Tag;

            lblSITName.Text =GameGrid.SelectedRows[0].Cells[1].Value.ToString();
            
            RvGameFullInfo tGame=new RvGameFullInfo();

            if (tGame == null)
            {
                lblSIDescription.Visible = false;
                lblSITDescription.Visible = false;

                lblSIManufacturer.Visible = false;
                lblSITManufacturer.Visible = false;

                lblSICloneOf.Visible = false;
                lblSITCloneOf.Visible = false;

                lblSIRomOf.Visible = false;
                lblSITRomOf.Visible = false;

                lblSIYear.Visible = false;
                lblSITYear.Visible = false;

                lblSITotalRoms.Visible = false;
                lblSITTotalRoms.Visible = false;

                // Trurip

                lblSIPublisher.Visible = false;
                lblSITPublisher.Visible = false;

                lblSIDeveloper.Visible = false;
                lblSITDeveloper.Visible = false;

                lblSIEdition.Visible = false;
                lblSITEdition.Visible = false;

                lblSIVersion.Visible = false;
                lblSITVersion.Visible = false;

                lblSIType.Visible = false;
                lblSITType.Visible = false;

                lblSIMedia.Visible = false;
                lblSITMedia.Visible = false;

                lblSILanguage.Visible = false;
                lblSITLanguage.Visible = false;

                lblSIPlayers.Visible = false;
                lblSITPlayers.Visible = false;

                lblSIRatings.Visible = false;
                lblSITRatings.Visible = false;

                lblSIGenre.Visible = false;
                lblSITGenre.Visible = false;

                lblSIPeripheral.Visible = false;
                lblSITPeripheral.Visible = false;

                lblSIBarCode.Visible = false;
                lblSITBarCode.Visible = false;

                lblSIMediaCatalogNumber.Visible = false;
                lblSITMediaCatalogNumber.Visible = false;
            }
            else
            {

                if (tGame.IsTrurip)
                {
                    lblSIDescription.Visible = true;
                    lblSITDescription.Visible = true;
                    lblSITDescription.Text = tGame.Description;

                    lblSIManufacturer.Visible = false;
                    lblSITManufacturer.Visible = false;

                    lblSICloneOf.Visible = false;
                    lblSITCloneOf.Visible = false;

                    lblSIRomOf.Visible = false;
                    lblSITRomOf.Visible = false;

                    lblSIYear.Visible = false;
                    lblSITYear.Visible = false;

                    lblSITotalRoms.Visible = false;
                    lblSITTotalRoms.Visible = false;


                    lblSIPublisher.Visible = true;
                    lblSITPublisher.Visible = true;
                    lblSITPublisher.Text = tGame.Publisher;

                    lblSIDeveloper.Visible = true;
                    lblSITDeveloper.Visible = true;
                    lblSITDeveloper.Text = tGame.Developer;

                    lblSIEdition.Visible = true;
                    lblSITEdition.Visible = true;
                    lblSITEdition.Text = tGame.Edition;

                    lblSIVersion.Visible = true;
                    lblSITVersion.Visible = true;
                    lblSITVersion.Text = tGame.Version;

                    lblSIType.Visible = true;
                    lblSITType.Visible = true;
                    lblSITType.Text = tGame.Type;

                    lblSIMedia.Visible = true;
                    lblSITMedia.Visible = true;
                    lblSITMedia.Text = tGame.Media;

                    lblSILanguage.Visible = true;
                    lblSITLanguage.Visible = true;
                    lblSITLanguage.Text = tGame.Language;

                    lblSIPlayers.Visible = true;
                    lblSITPlayers.Visible = true;
                    lblSITPlayers.Text = tGame.Players;

                    lblSIRatings.Visible = true;
                    lblSITRatings.Visible = true;
                    lblSITRatings.Text = tGame.Ratings;

                    lblSIGenre.Visible = true;
                    lblSITGenre.Visible = true;
                    lblSITGenre.Text = tGame.Genre;

                    lblSIPeripheral.Visible = true;
                    lblSITPeripheral.Visible = true;
                    lblSITPeripheral.Text = tGame.Peripheral;

                    lblSIBarCode.Visible = true;
                    lblSITBarCode.Visible = true;
                    lblSITBarCode.Text = tGame.BarCode;

                    lblSIMediaCatalogNumber.Visible = true;
                    lblSITMediaCatalogNumber.Visible = true;
                    lblSITMediaCatalogNumber.Text = tGame.MediaCatalogNumber;

                }
                else
                {
                    lblSIDescription.Visible = true;
                    lblSITDescription.Visible = true;
                    lblSITDescription.Text = tGame.Description;

                    lblSIManufacturer.Visible = true;
                    lblSITManufacturer.Visible = true;
                    lblSITManufacturer.Text = tGame.Manufacturer;

                    lblSICloneOf.Visible = true;
                    lblSITCloneOf.Visible = true;
                    lblSITCloneOf.Text = tGame.CloneOf;

                    lblSIRomOf.Visible = true;
                    lblSITRomOf.Visible = true;
                    lblSITRomOf.Text = tGame.RomOf;

                    lblSIYear.Visible = true;
                    lblSITYear.Visible = true;
                    lblSITYear.Text = tGame.Year;

                    lblSITotalRoms.Visible = true;
                    lblSITTotalRoms.Visible = true;




                    lblSIPublisher.Visible = false;
                    lblSITPublisher.Visible = false;

                    lblSIDeveloper.Visible = false;
                    lblSITDeveloper.Visible = false;

                    lblSIEdition.Visible = false;
                    lblSITEdition.Visible = false;

                    lblSIVersion.Visible = false;
                    lblSITVersion.Visible = false;

                    lblSIType.Visible = false;
                    lblSITType.Visible = false;

                    lblSIMedia.Visible = false;
                    lblSITMedia.Visible = false;

                    lblSILanguage.Visible = false;
                    lblSITLanguage.Visible = false;

                    lblSIPlayers.Visible = false;
                    lblSITPlayers.Visible = false;

                    lblSIRatings.Visible = false;
                    lblSITRatings.Visible = false;

                    lblSIGenre.Visible = false;
                    lblSITGenre.Visible = false;

                    lblSIPeripheral.Visible = false;
                    lblSITPeripheral.Visible = false;

                    lblSIBarCode.Visible = false;
                    lblSITBarCode.Visible = false;

                    lblSIMediaCatalogNumber.Visible = false;
                    lblSITMediaCatalogNumber.Visible = false;

                }
            }

            


            UpdateRomGrid(GameId);
        }

        #endregion

        #region Rom display code

        private void UpdateRomGrid(int GameId)
        { }

        #endregion
    }
}
