﻿using System;
using System.ComponentModel;
using System.IO;
using RomVaultX.DB;
using RomVaultX.Util;

namespace RomVaultX.DatReader
{
    public static class DatCmpReader
    {
        public static bool ReadDat(string strFilename, long fileTimeStamp, out RvDat rvDat)
        {
            rvDat = new RvDat();
            int errorCode = DatFileLoader.LoadDat(strFilename);
            if (errorCode != 0)
            {
                DatUpdate.ShowDat(new Win32Exception(errorCode).Message, strFilename);
                return false;
            }

            string Filename = IO.Path.GetFileName(strFilename);

            DatFileLoader.Gn();
            if (DatFileLoader.EndOfStream())
                return false;
            if (DatFileLoader.Next.ToLower() == "clrmamepro")
            {
                DatFileLoader.Gn();
                if (!LoadHeaderFromDat(Filename, fileTimeStamp, rvDat))
                    return false;
                DatFileLoader.Gn();
            }
            if (DatFileLoader.Next.ToLower() == "romvault")
            {
                DatFileLoader.Gn();
                if (!LoadHeaderFromDat(Filename, fileTimeStamp, rvDat))
                    return false;
                DatFileLoader.Gn();
            }

            while (!DatFileLoader.EndOfStream())
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "dir":
                        DatFileLoader.Gn();
                        if (!LoadDirFromDat(rvDat, ""))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "game":
                        DatFileLoader.Gn();
                        if (!LoadGameFromDat(rvDat, ""))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "resource":
                        DatFileLoader.Gn();
                        if (!LoadGameFromDat(rvDat, ""))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "emulator":
                        DatFileLoader.Gn();
                        if (!LoadEmulator())
                            return false;
                        DatFileLoader.Gn();
                        break;
                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not known", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }

            DatFileLoader.Close();

            return true;
        }


        private static bool LoadHeaderFromDat(string Filename, long fileTimeStamp, RvDat rvDat)
        {
            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after clrmamepro", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            rvDat.Filename = Filename;
            rvDat.DatTimeStamp = fileTimeStamp;

            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "name": rvDat.Name = VarFix.CleanFileName(DatFileLoader.GnRest()); DatFileLoader.Gn(); break;
                    case "description": rvDat.Description = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "category": rvDat.Category = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "version": rvDat.Version = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "date": rvDat.Date = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "author": rvDat.Author = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "email": rvDat.Email = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "homepage": rvDat.Homepage = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "url": rvDat.URL = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;

                    case "comment": rvDat.Comment = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "header": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "forcezipping": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "forcepacking": DatFileLoader.GnRest(); DatFileLoader.Gn(); break; // incorrect usage
                    case "forcemerging": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "forcenodump": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "dir": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not known in clrmamepro", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }

            return true;

        }

        private static bool LoadEmulator()
        {
            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after emulator", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();
            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "name": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "version": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                }
            }
            return true;
        }



        private static bool LoadDirFromDat(RvDat rvDat, string rootDir)
        {
            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after game", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            if (DatFileLoader.Next.ToLower() != "name")
            {
                DatUpdate.SendAndShowDat("Name not found as first object in ( )", DatFileLoader.Filename);
                return false;
            }
            string fullname = VarFix.CleanFullFileName(DatFileLoader.GnRest());

            DatFileLoader.Gn();

            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "dir":
                        DatFileLoader.Gn();
                        if (!LoadDirFromDat(rvDat, fullname))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "game":
                        DatFileLoader.Gn();
                        if (!LoadGameFromDat(rvDat, fullname))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "resource":
                        DatFileLoader.Gn();
                        if (!LoadGameFromDat(rvDat, fullname))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    default:
                        DatUpdate.SendAndShowDat("Error Keyword " + DatFileLoader.Next + " not know in dir", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }
            return true;
        }

        private static bool LoadGameFromDat(RvDat rvDat, string rootdir)
        {
            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after game", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            string snext = DatFileLoader.Next.ToLower();

            string pathextra = "";
            if (snext == "rebuildto")
            {
                pathextra = VarFix.CleanFullFileName(DatFileLoader.Gn()); DatFileLoader.Gn();
                snext = DatFileLoader.Next.ToLower();
            }

            if (snext != "name")
            {
                DatUpdate.SendAndShowDat("Name not found as first object in ( )", DatFileLoader.Filename);
                return false;
            }


            string name = VarFix.CleanFullFileName(DatFileLoader.GnRest());

            if (!String.IsNullOrEmpty(pathextra))
                name = pathextra + "/" + name;

            DatFileLoader.Gn();

            RvGame rvGame = new RvGame();
            rvGame.Name = name;
            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "romof": rvGame.RomOf = VarFix.CleanFileName(DatFileLoader.GnRest()); DatFileLoader.Gn(); break;
                    case "description": rvGame.Description = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;

                    case "sourcefile": rvGame.SourceFile = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "cloneof": rvGame.CloneOf = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "sampleof": rvGame.SampleOf = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "board": rvGame.Board = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "year": rvGame.Year = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "manufacturer": rvGame.Manufacturer = DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "serial": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "rebuildto": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;

                    case "sample": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "biosset": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;

                    case "chip": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "video": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "sound": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "input": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "dipswitch": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;
                    case "driver": DatFileLoader.GnRest(); DatFileLoader.Gn(); break;


                    case "rom":
                        DatFileLoader.Gn();
                        if (!LoadRomFromDat(rvGame))
                            return false;
                        DatFileLoader.Gn();
                        break;
                    case "disk":
                        DatFileLoader.Gn();
                        if (!LoadDiskFromDat(rvGame))
                            return false;
                        DatFileLoader.Gn();
                        break;

                    case "archive":
                        DatFileLoader.Gn();
                        if (!LoadArchiveFromDat())
                            return false;
                        DatFileLoader.Gn();
                        break;

                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not known in game", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }
            rvDat.AddGame(rvGame);
            return true;
        }

        private static bool LoadRomFromDat(RvGame rvGame)
        {

            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after rom", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            if (DatFileLoader.Next.ToLower() != "name")
            {
                DatUpdate.SendAndShowDat("Name not found as first object in ( )", DatFileLoader.Filename);
                return false;
            }


            RvRom rvRom = new RvRom();
            rvRom.Name = VarFix.CleanFullFileName(DatFileLoader.Gn());
            DatFileLoader.Gn();


            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "size": rvRom.Size = VarFix.ULong(DatFileLoader.Gn()); DatFileLoader.Gn(); break;
                    case "crc": rvRom.CRC = VarFix.CleanMD5SHA1(DatFileLoader.Gn(), 8); DatFileLoader.Gn(); break;
                    case "sha1": rvRom.SHA1 = VarFix.CleanMD5SHA1(DatFileLoader.Gn(), 40); DatFileLoader.Gn(); break;
                    case "md5": rvRom.MD5 = VarFix.CleanMD5SHA1(DatFileLoader.Gn(), 32); DatFileLoader.Gn(); break;
                    case "merge": rvRom.Merge = VarFix.CleanFullFileName(DatFileLoader.Gn()); DatFileLoader.Gn(); break;
                    case "flags": rvRom.Status = VarFix.ToLower(DatFileLoader.Gn()); DatFileLoader.Gn(); break;
                    case "date": DatFileLoader.Gn(); DatFileLoader.Gn(); break;
                    case "bios": DatFileLoader.Gn(); DatFileLoader.Gn(); break;
                    case "region": DatFileLoader.Gn(); DatFileLoader.Gn(); break;
                    case "offs": DatFileLoader.Gn(); DatFileLoader.Gn(); break;
                    case "nodump": rvRom.Status = "nodump"; DatFileLoader.Gn(); break;
                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not known in rom", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }

            rvGame.AddRom(rvRom);

            return true;
        }

        private static bool LoadDiskFromDat(RvGame rvGame)
        {

            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after rom", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            if (DatFileLoader.Next.ToLower() != "name")
            {
                DatUpdate.SendAndShowDat("Name not found as first object in ( )", DatFileLoader.Filename);
                return false;
            }


            string filename = VarFix.CleanFullFileName(DatFileLoader.Gn());
            byte[] sha1;
            byte[] md5;
            string Merge;
            string Status;

            DatFileLoader.Gn();

            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "sha1": sha1 = VarFix.CleanMD5SHA1(DatFileLoader.Gn(), 40); DatFileLoader.Gn(); break;
                    case "md5": md5 = VarFix.CleanMD5SHA1(DatFileLoader.Gn(), 32); DatFileLoader.Gn(); break;
                    case "merge": Merge = VarFix.CleanFullFileName(DatFileLoader.Gn()); DatFileLoader.Gn(); break;
                    case "flags": Status = VarFix.ToLower(DatFileLoader.Gn()); DatFileLoader.Gn(); break;
                    case "nodump": Status = "nodump"; DatFileLoader.Gn(); break;
                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not known in rom", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }

            return true;
        }

        private static bool LoadArchiveFromDat()
        {

            if (DatFileLoader.Next != "(")
            {
                DatUpdate.SendAndShowDat("( not found after Archive", DatFileLoader.Filename);
                return false;
            }
            DatFileLoader.Gn();

            while (DatFileLoader.Next != ")")
            {
                switch (DatFileLoader.Next.ToLower())
                {
                    case "name": DatFileLoader.Gn(); DatFileLoader.Gn(); break;
                    default:
                        DatUpdate.SendAndShowDat("Error: key word '" + DatFileLoader.Next + "' not know in Archive", DatFileLoader.Filename);
                        DatFileLoader.Gn();
                        break;
                }
            }
            return true;
        }



        private static class DatFileLoader
        {
            public static String Filename { get; private set; }
            private static Stream _fileStream;
            private static StreamReader _streamReader;
            private static string _line = "";
            public static string Next;

            public static int LoadDat(string strFilename)
            {
                Filename = strFilename;
                _streamReader = null;
                int errorCode = IO.FileStream.OpenFileRead(strFilename, out _fileStream);
                if (errorCode != 0)
                    return errorCode;
                _streamReader = new StreamReader(_fileStream, Program.Enc);
                return 0;
            }
            public static void Close()
            {
                _streamReader.Close();
                _fileStream.Close();
                _streamReader.Dispose();
                _fileStream.Dispose();
            }

            public static bool EndOfStream()
            {
                return _streamReader.EndOfStream;
            }

            public static string GnRest()
            {
                string strret = _line.Replace("\"", "");
                _line = "";
                Next = strret;
                return strret;
            }

            public static string Gn()
            {
                string ret;
                while ((_line.Trim().Length == 0) && (!_streamReader.EndOfStream))
                {
                    _line = _streamReader.ReadLine();
                    _line = (_line ?? "").Replace("" + (char)9, " ");
                    if (_line.TrimStart().Length > 2 && _line.TrimStart().Substring(0, 2) == @"//") _line = "";
                    if (_line.TrimStart().Length > 1 && _line.TrimStart().Substring(0, 1) == @"#") _line = "";
                    if (_line.TrimStart().Length > 1 && _line.TrimStart().Substring(0, 1) == @";") _line = "";
                    _line = _line.Trim() + " ";
                }

                if (_line.Trim().Length > 0)
                {
                    int intS;
                    if (_line.Substring(0, 1) == "\"")
                    {
                        intS = (_line + "\"").IndexOf("\"", 1, StringComparison.Ordinal);
                        ret = _line.Substring(1, intS - 1);
                        _line = (_line + " ").Substring(intS + 1).Trim();
                    }
                    else
                    {
                        intS = (_line + " ").IndexOf(" ", StringComparison.Ordinal);
                        ret = _line.Substring(0, intS);
                        _line = (_line + " ").Substring(intS).Trim();
                    }
                }
                else
                {
                    ret = "";
                }

                Next = ret;
                return ret;
            }
        }

    }
}
