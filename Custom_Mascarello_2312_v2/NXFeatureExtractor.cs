using Newtonsoft.Json;
using NXOpen;
using NXOpen.CAE;
using NXOpen.Features;
using NXOpen.Features.ShipDesign;
using NXOpen.UF;
using NXOpen.Weld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static NXOpen.UF.UFModl;

namespace Custom_Mascarello
{
    public class NXFeatureExtractor
    {
        private Session theSession;
        private UFSession theUFSession;

        public NXFeatureExtractor()
        {
            theSession = Session.GetSession();
            theUFSession = UFSession.GetUFSession();
        }
        public void ExtractAllPartsToJson(string codigo, string nl)
        {
           // MessageBox.Show(" ITEM ->" + codigo);
            theSession = Session.GetSession();
            theUFSession = UFSession.GetUFSession();
            string item = Create.TCSearchItem(codigo);
          //  MessageBox.Show(" ITEM ->" + item);


            PartLoadStatus LdSt;
            Part obj_familia;


            string directory = @"D:\Temp\Dados";

            try
            {
                obj_familia = (Part)theSession.Parts.FindObject(item);
            }
            catch
            {
                obj_familia = theSession.Parts.Open(item, out LdSt);
            }

            string jsonData = ExtractPartData(obj_familia, nl);

          //  MessageBox.Show(" DADOS ->" + jsonData);
            string jsonFileName = Path.Combine(
           directory,
                    codigo+ ".json"
                );

            File.WriteAllText(jsonFileName, jsonData);

            theSession.Parts.CloseAll(NXOpen.BasePart.CloseModified.CloseModified, null);


            // Fechar a peça para liberar memória
            //theSession.Parts.CloseAll(false, false);
            //theSession.Parts.Close(obj_familia, false, false);
            // Obter todos os arquivos .prt no diretório
            //string[] partFiles = Directory.GetFiles(directory, "*.prt", SearchOption.AllDirectories);

            //foreach (string partFile in partFiles)
            //{
            //    try
            //    {
            //        Part part = theSession.Parts.OpenRead(partFile);
            //        theSession.Parts.SetWork(part);

            //        string jsonData = ExtractPartData(part);

            //        // Criar nome de arquivo JSON baseado no nome da peça
            //        string jsonFileName = Path.Combine(
            //            Path.GetDirectoryName(partFile),
            //            Path.GetFileNameWithoutExtension(partFile) + ".json"
            //        );

            //        File.WriteAllText(jsonFileName, jsonData);

            //        Console.WriteLine($"Extraído com sucesso: {partFile}");

            //        // Fechar a peça para liberar memória
            //        theSession.Parts.Close(part, false, false);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Erro ao processar {partFile}: {ex.Message}");
            //    }
            //}
        }
        private string ExtractPartData(Part part, string nl)
        {
            frmPrincipal.Registrar_no_LOG(" DADOS ->" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "ExtractPartData");
            var partData = new Dictionary<string, object>();

            //try
            //{
                // 1. Extrair metadados
                partData["metadata"] = ExtractMetadata(part, nl);

                // 2. Extrair dimensões principais
              //  partData["dimensoes_principais"] = ExtractMainDimensions(part);

                //// 3. Extrair árvore de features
                partData["arvore_features"] = ExtractFeatureTree(part);

                //// 4. Extrair expressões
                //partData["expressoes"] = ExtractExpressions(part);

                // Converter para JSON formatado
            //}
            //catch 
            //{

              
            //}
            
            return JsonConvert.SerializeObject(partData, Formatting.Indented);
        }

        private List<Dictionary<string, object>>    ExtractFeatureTree(Part part)
        {
            var featureTree = new List<Dictionary<string, object>>();

            // Percorrer todas as features na ordem de criação  
            foreach (Feature feature in part.Features)
            {
                //try
                //{
                    var featureData = new Dictionary<string, object>();

                    featureData["id"] = feature.JournalIdentifier;
                    featureData["nome"] = feature.Name;
                    featureData["tipo"] = feature.GetType().Name;

                // Verificar se a feature é um esboço antes de tentar convertê-la  
                if (feature is SketchFeature sketchFeature)
                {
                    Sketch sketch = sketchFeature.Sketch;
                    featureData["elementos"] = ExtractSketchElements(sketch);
                }

                // Adicionar informações de feature pai/base quando aplicável  
                //Feature[] parents = feature.GetParents();
                //if (parents != null && parents.Length > 0)
                //{
                //    featureData["feature_base"] = parents[0].JournalIdentifier;
                //}
                //if (GetFeatureType(feature) != "brep")
                //{
                featureTree.Add(featureData);
                //}
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Erro ao processar feature {feature.Name}: {ex.Message}");
                //}
            }

            return featureTree;
        }

        private string GetFeatureType(Feature feature)
        {

            // Mapear tipos de feature do NX para nomes mais amigáveis
            //if (feature is Extrude ) return "extrude";
            //if (feature is Revolve) return "revolve";
            //if (feature is Sketch) return "sketch";
            //if (feature is Hole) return "hole";
            //if (feature is PatternFeature) return "pattern";
            //if (feature is Fillet) return "fillet";
            //if (feature is Chamfer) return "chamfer";
            // Adicionar outros tipos conforme necessário

            return feature.GetType().Name.Replace("Feature", "").ToLower();
        }

       


        private Dictionary<string, double> ExtractMainDimensions(Part part)
        {
            frmPrincipal.Registrar_no_LOG(" DADOS ->" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "ExtractPartData");
            var dimensions = new Dictionary<string, double>();

            try
            {
                Body[] bodies = part.Bodies.ToArray();

                if (bodies.Length > 0)
                {
                    double minX = double.MaxValue, maxX = double.MinValue;
                    double minY = double.MaxValue, maxY = double.MinValue;
                    double minZ = double.MaxValue, maxZ = double.MinValue;

                    foreach (Body body in bodies)
                    {
                        if (body.IsSolidBody)
                        {
                            try
                            {
                                Tag bodyTag = body.Tag;

                                // Parâmetros para AskBoundingBoxExact
                                double[] minCorner = new double[3];
                                double[] maxCorner = new double[3];
                                double[,] orientMatrix = new double[3, 3];

                                // O método é void, não retorna código de erro
                                theUFSession.Modl.AskBoundingBoxExact(
                                    bodyTag,
                                    Tag.Null, // csys_tag
                                    minCorner,
                                    orientMatrix,
                                    maxCorner
                                );

                                // Verificar se os valores são válidos
                                if (IsValidBoundingBox(minCorner, maxCorner))
                                {
                                    minX = Math.Min(minX, minCorner[0]);
                                    maxX = Math.Max(maxX, maxCorner[0]);
                                    minY = Math.Min(minY, minCorner[1]);
                                    maxY = Math.Max(maxY, maxCorner[1]);
                                    minZ = Math.Min(minZ, minCorner[2]);
                                    maxZ = Math.Max(maxZ, maxCorner[2]);
                                }
                            }
                            catch (Exception bodyEx)
                            {
                                Console.WriteLine($"Erro ao processar body: {bodyEx.Message}");
                            }
                        }
                    }

                    if (minX != double.MaxValue)
                    {
                        dimensions["comprimento"] = Math.Abs(maxX - minX);
                        dimensions["largura"] = Math.Abs(maxY - minY);
                        dimensions["altura"] = Math.Abs(maxZ - minZ);
                    }
                }

               // ExtractMassProperties(part, dimensions);
            }
            catch (Exception ex)
            {
                
               // SetDefaultDimensions(dimensions);
            }

            return dimensions;
        }

        private List<Dictionary<string, object>> ExtractSketchElements(Sketch sketch)
        {
            var elements = new List<Dictionary<string, object>>();

            //try
            //{
                NXOpen.Curve[] curves = sketch.GetAllGeometry().OfType<NXOpen.Curve>().ToArray();
                

                foreach (NXOpen.Curve curve in curves)
                {
                    var elementData = new Dictionary<string, object>();
                    
                elementData["tipo"] = curve.GetType().Name;
                foreach (var param in curve.GetType().GetProperties())
                {
                    try
                    {
                        object value = param.GetValue(curve);
                        if (value is double || value is int || value is string)
                        {

                            elementData[param.Name] = value;
                        }
                        else
                        {
                            elementData[param.Name] = value.ToString();
                        }
                    }
                    catch
                    {
                        // Ignorar propriedades que não podem ser acessadas
                    }
                }
                //if (curve is NXOpen.Line)
                //{
                //    NXOpen.Line line = (NXOpen.Line)curve;
                //    //MessageBox.Show(line.GetLength().ToString());
                //    elementData["tipo"] = "linha";
                //    elementData["parametros"] = new Dictionary<string, double>
                //    {
                //        ["x1"] = line.StartPoint.X,
                //        ["y1"] = line.StartPoint.Y,
                //        ["x2"] = line.EndPoint.X,
                //        ["y2"] = line.EndPoint.Y,
                //        ["length"] = line.GetLength()
                //    };

                //}
                //else if (curve is NXOpen.Arc)
                //{
                //    NXOpen.Arc arc = (NXOpen.Arc)curve;
                //    elementData["tipo"] = "arco";
                //    elementData["parametros"] = new Dictionary<string, double>
                //    {
                //        ["cx"] = arc.CenterPoint.X,
                //        ["cy"] = arc.CenterPoint.Y,
                //        ["raio"] = arc.Radius,
                //        ["angInicial"] = arc.StartAngle,
                //        ["angFinal"] = arc.EndAngle
                //      //  ["length"] = arc.GetLength()
                //    };
                //}

                // Caso outros elementos sejam suportados, adicione outros else if
                // Exemplo: splines, elipses...

                // Adiciona o elemento encontrado
                elements.Add(elementData);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Erro ao extrair elementos do sketch: {ex.Message}");
            //}

            return elements;
        }

        private bool IsValidBoundingBox(double[] minCorner, double[] maxCorner)
        {
            // Verificar se os valores são válidos (não são NaN ou infinitos)
            for (int i = 0; i < 3; i++)
            {
                if (double.IsNaN(minCorner[i]) || double.IsInfinity(minCorner[i]) ||
                    double.IsNaN(maxCorner[i]) || double.IsInfinity(maxCorner[i]))
                {
                    return false;
                }
            }

            // Verificar se max > min
            return maxCorner[0] > minCorner[0] &&
                   maxCorner[1] > minCorner[1] &&
                   maxCorner[2] > minCorner[2];
        }

        private Dictionary<string, object> ExtractMetadata(Part part, string nl)
        {
            var metadata = new Dictionary<string, object>();

            // Extrair código e descrição dos atributos do NX
            string codigo = part.GetStringAttribute("DB_PART_NO");
            string descricao = part.GetStringAttribute("DB_PART_NAME");
           
            metadata["codigo"] = codigo;
            metadata["descricao"] = descricao;
            metadata["nl"] = nl;

            // Determinar tipo de componente baseado em análise da geometria


            return metadata;
        }

    }
    
}
