using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Euston.Business
{
    public class ProcessMessageHelper
    {
        [ImportMany]
        public System.Lazy<IProcessMessage, IDictionary<string, object>>[] CalPlugins { get; set; }

        public void AssembleComponents()
        {
            try
            {
                var aggregateCatalog = new AggregateCatalog();

                var directoryPath = @"c:\";
                var directoryCatalog = new DirectoryCatalog(directoryPath, "*.dll");

                var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

                aggregateCatalog.Catalogs.Add(directoryCatalog);
                aggregateCatalog.Catalogs.Add(asmCatalog);

                var container = new CompositionContainer(aggregateCatalog);
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public List<string> Execute(string messageId, string messageBody, MessageType.Type messageType)
        {
            List<string> errors;
            List<string> message = new List<string>();
            foreach (var CalPlugin in CalPlugins)
            {
                if ((string)CalPlugin.Metadata["MessageMetaData"] == messageType.ToString())
                {
                    errors = CalPlugin.Value.RunValidation(messageBody);
                    if (errors.Any())
                    {
                        MessageBox.Show(string.Join(System.Environment.NewLine, errors), "Validation Error",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
                        break;
                    }
                    message = CalPlugin.Value.ProcessMessage(messageId, messageBody);
                    break;
                }
            }
            return message;
        }
    }
}
