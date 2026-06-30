using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Mascarello
{
    public class item_tarefas
    {
        public class Item
        {
            //public string oitem_id { get; set; }
            public string oitem_pk_orcamento { get; set; }
            public string oitem_pk_grupo { get; set; }
            //public string oitem_pk_subgrupo { get; set; }
            //public string oitem_pk_item { get; set; }
            //public string oitem_item { get; set; }
            //public string oitem_item_es { get; set; }
            //public string oitem_tempo { get; set; }
            //public string oitem_tempo_custo { get; set; }
            //public string oitem_item_custo { get; set; }
            //public string oitem_item_custo_total { get; set; }
            //public string oitem_item_valor { get; set; }
            //public string oitem_zerado { get; set; }
            //public string sub_id { get; set; }
            //public DateTime sub_datacad { get; set; }
            public string sub_grupo { get; set; }
            public string sub_referencia { get; set; }
            public string sub_titulo { get; set; }
            //public string sub_formato { get; set; }
            //public string sub_conf_replica { get; set; }
            //public string sub_referencia_apartir { get; set; }
            //public string sub_planta { get; set; }
            //public string sub_exibir_valor { get; set; }
            //public DateTime sub_vigencia_inicio { get; set; }
            //public DateTime sub_vigencia_fim { get; set; }
            //public string sub_descricao { get; set; }
            //public string sub_conf_acima_ref { get; set; }
            //public string gru_id { get; set; }
            //public DateTime gru_datacad { get; set; }
            //public string gru_referencia { get; set; }
            public string gru_titulo { get; set; }
            //public DateTime gru_vigencia_inicio { get; set; }
            //public DateTime gru_vigencia_fim { get; set; }
            public string item_id { get; set; }
            //public DateTime item_datacad { get; set; }
            public string item_referencia { get; set; }
            public string item_titulo { get; set; }
            //public string item_titulo_es { get; set; }
            //public string item_abr_programacao { get; set; }
            //public string item_abr_dna { get; set; }
            public string item_grupo { get; set; }
            public string nome_variavel { get; set; }
            public string valor_variavel { get; set; }

            //public string item_subgrupo { get; set; }
            //public string item_configuracao { get; set; }
            //public DateTime item_vigencia_inicio { get; set; }
            //public DateTime item_vigencia_fim { get; set; }
            //public string item_ordem { get; set; }
            //public string item_texto_pt { get; set; }
            //public string item_texto_es { get; set; }
        }
    }
}
