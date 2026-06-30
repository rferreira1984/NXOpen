using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Windows;

namespace Custom_Mascarello
{
    class Clean
    {
        private static Session theSession;
        private static UI theUI;
        private static UFSession theUfSession;

        public static List<String> Lista_itens_supressed = new List<String>();

        public static string clean_supressed(string delete)
        {
            string arq_ret = "";


            theSession = Session.GetSession();
            Part wp_m = theSession.Parts.Work;

            List<NXOpen.Assemblies.Component> Componentes = new List<NXOpen.Assemblies.Component>();

            NXOpen.Assemblies.Component cmp = wp_m.ComponentAssembly.RootComponent;

            Componentes.Add(cmp);

            string codigo_item = "";
            string total_itens = "";
            Componentes.ToArray()[0].GetChildren();

            for (int i = 0; i < 1; i++)
            {
                NXOpen.Assemblies.Component[] cmps = Componentes.ToArray()[i].GetChildren();
                foreach (NXOpen.Assemblies.Component cmp_1 in cmps)
                {
                    NXObject[] objects1 = new NXObject[1];

                    if (cmp_1.IsSuppressed)
                    {

                        
                        int cont = 0;




                        NXOpen.Positioning.ComponentConstraint[] componentConstraint1 = cmp_1.GetConstraints();
                        //foreach (var item in componentConstraint1)
                        //{
                        //    cont++;
                        //}
                        

                        int _cont_1 = 0;
                        foreach (var item in componentConstraint1)
                        { 
                            NXObject[] objects2 = new NXObject[1];
                            objects2[0] = item;
                            int nErrs2;
                            nErrs2 = theSession.UpdateManager.AddToDeleteList(objects2);
                            
                        }

                        
                        objects1[0] = cmp_1;
                        NXOpen.Session.UndoMarkId markId1;
                        markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");
                       
                        NXOpen.Session.UndoMarkId markId2;
                        markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

                        int nErrs1;
                        nErrs1 = theSession.UpdateManager.AddToDeleteList(objects1);



                        int nErrs3;
                        nErrs3 = theSession.UpdateManager.DoUpdate(markId2);

                        theSession.DeleteUndoMark(markId1, null);
                    }
                }
              
            }
            return arq_ret;
        }
    }
}

