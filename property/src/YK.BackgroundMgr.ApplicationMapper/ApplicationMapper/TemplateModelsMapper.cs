using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.ApplicationMapper
{
   public class TemplateModelsMapper
    {
        public static IEnumerable<TemplateModel> ChangeTemplateModelToDTOs(IEnumerable<TemplateModel> templateModels)
        {
            //配置AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TemplateModel, TemplateModel>();
                cfg.CreateMap<IOrderedEnumerable<DictionaryModel>, IOrderedEnumerable<DictionaryModel>>();
            });
            var tempTemplateModels = Mapper.Map<IEnumerable<TemplateModel>, IEnumerable<TemplateModel>>(templateModels);

            return tempTemplateModels;
        }
    }
}
