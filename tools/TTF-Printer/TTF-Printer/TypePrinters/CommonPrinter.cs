using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using IWA.TTF.Taxonomy.Model.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.Collections;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class CommonPrinter
    {
        private static readonly ILog _log;
        static CommonPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void BuildInvocationsTable(WordprocessingDocument document, IEnumerable<Invocation> invocations)
        {
            _log.Info("Printing InvocationsTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text("Invocations"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", invokes);

            foreach (var i in invocations) {

                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text(i.Name));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", exBody);

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Id: " + i.Id));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Description: " + i.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                var requestBody = body.AppendChild(new Paragraph());
                var requestRun = requestBody.AppendChild(new Run());
                requestRun.AppendChild(new Text("Request"));
                Utils.ApplyStyleToParagraph(document, "Heading5", "Heading5", requestBody);

                var reqBody = body.AppendChild(new Paragraph());
                var reqRun = reqBody.AppendChild(new Run());
                reqRun.AppendChild(new Text("Control Message: " + i.Request.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", reqBody);

                var dBody = body.AppendChild(new Paragraph());
                var dRun = dBody.AppendChild(new Run());
                dRun.AppendChild(new Text("Description: " + i.Request.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", dBody);

                var paramsBody = body.AppendChild(new Paragraph());
                var paramsRun = paramsBody.AppendChild(new Run());
                paramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading6", "Heading6", paramsBody);

                var invokePara = body.AppendChild(new Paragraph());
                var invokeRun = invokePara.AppendChild(new Run());
                invokeRun.AppendChild(GetParamsTable(document, i.Request.InputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", invokePara);

                //response
                var responseBody = body.AppendChild(new Paragraph());
                var responseRun = responseBody.AppendChild(new Run());
                responseRun.AppendChild(new Text("Response"));
                Utils.ApplyStyleToParagraph(document, "Heading5", "Heading5", responseBody);

                var resBody = body.AppendChild(new Paragraph());
                var resRun = resBody.AppendChild(new Run());
                resRun.AppendChild(new Text("Control Message: " + i.Response.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", resBody);

                var rBody = body.AppendChild(new Paragraph());
                var rRun = rBody.AppendChild(new Run());
                rRun.AppendChild(new Text("Description: " + i.Response.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rBody);

                var ramsBody = body.AppendChild(new Paragraph());
                var ramsRun = ramsBody.AppendChild(new Run());
                ramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading6", "Heading6", ramsBody);

                var rinvokePara = body.AppendChild(new Paragraph());
                var rinvokeRun = rinvokePara.AppendChild(new Run());
                rinvokeRun.AppendChild(GetParamsTable(document, i.Response.OutputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rinvokePara);

            }
        }

        public static void BuildInvocationBindingsTable(WordprocessingDocument document,
            IEnumerable<InvocationBinding> invocations, string behaviorName)
        {
            _log.Info("Printing Invocation Bindings Table");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text(behaviorName + " responds to these Invocations"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", invokes);

            foreach (var i in invocations)
            {
                if (i.InvocationStep.NextInvocation == null)
                {
                    BuildInvocation(document, i.InvocationStep.Invocation);
                    continue;
                }

                var influencingBehaviorName = ModelManager.GetBehaviorArtifact(new ArtifactSymbol
                {
                    Id = i.Influence.InfluencingId
                }).Artifact.Name;
                var influencingBehaviorInvocationName= ModelManager.GetBehaviorArtifact(new ArtifactSymbol
                {
                    Id = i.Influence.InfluencingId
                }).Invocations.SingleOrDefault(e=>e.Id == i.Influence.InfluencingInvocationId)
                    ?.Name;
                    
                var bindPara = body.AppendChild(new Paragraph());
                var bindRun = bindPara.AppendChild(new Run());
                bindRun.AppendChild(new Text("Binding Is Influenced by " + influencingBehaviorName + "'s Invocation " +
                                             influencingBehaviorInvocationName));
                
                var bind2Run = bindPara.AppendChild(new Run());
                bind2Run.AppendChild(new Text(influencingBehaviorName + "'s Invocation " +
                                             influencingBehaviorInvocationName + " " + 
                                             i.Influence.InfluenceType + "s this behavior's invocation.'"));
                
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bindPara);
                
                BuildInvocation(document, i.InvocationStep.Invocation);
                if (i.InvocationStep.NextInvocation == null) continue;
                BuildInvocation(document, i.InvocationStep.NextInvocation.Invocation);
                if(i.InvocationStep.NextInvocation.NextInvocation !=null)
                    BuildInvocation(document, i.InvocationStep.NextInvocation.NextInvocation.Invocation);
            }
        }

        private static void BuildInvocation(WordprocessingDocument document, Invocation i)
        {
            var body = document.MainDocumentPart.Document.Body;
            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(i.Name));
            Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", exBody);

            var idBody = body.AppendChild(new Paragraph());
            var idRun = idBody.AppendChild(new Run());
            idRun.AppendChild(new Text("Id: " + i.Id));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

            var iDescription = body.AppendChild(new Paragraph());
            var iRun = iDescription.AppendChild(new Run());
            iRun.AppendChild(new Text("Description: " + i.Description));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

            var requestBody = body.AppendChild(new Paragraph());
            var requestRun = requestBody.AppendChild(new Run());
            requestRun.AppendChild(new Text("Request Message:"));
            Utils.ApplyStyleToParagraph(document, "Heading5", "Heading5", requestBody);

            var reqBody = body.AppendChild(new Paragraph());
            var reqRun = reqBody.AppendChild(new Run());
            reqRun.AppendChild(new Text(i.Request.ControlMessageName));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", reqBody);

            var dBody = body.AppendChild(new Paragraph());
            var dRun = dBody.AppendChild(new Run());
            dRun.AppendChild(new Text("Description: " + i.Request.Description));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", dBody);

            var paramsBody = body.AppendChild(new Paragraph());
            var paramsRun = paramsBody.AppendChild(new Run());
            paramsRun.AppendChild(new Text("Request Parameters"));
            Utils.ApplyStyleToParagraph(document, "Heading6", "Heading6", paramsBody);

            var invokePara = body.AppendChild(new Paragraph());
            var invokeRun = invokePara.AppendChild(new Run());
            invokeRun.AppendChild(GetParamsTable(document, i.Request.InputParameters));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", invokePara);

            //response
            var responseBody = body.AppendChild(new Paragraph());
            var responseRun = responseBody.AppendChild(new Run());
            responseRun.AppendChild(new Text("Response Message"));
            Utils.ApplyStyleToParagraph(document, "Heading5", "Heading5", responseBody);

            var resBody = body.AppendChild(new Paragraph());
            var resRun = resBody.AppendChild(new Run());
            resRun.AppendChild(new Text(i.Response.ControlMessageName));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", resBody);

            var rBody = body.AppendChild(new Paragraph());
            var rRun = rBody.AppendChild(new Run());
            rRun.AppendChild(new Text("Description: " + i.Response.Description));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rBody);

            var ramsBody = body.AppendChild(new Paragraph());
            var ramsRun = ramsBody.AppendChild(new Run());
            ramsRun.AppendChild(new Text("Response Parameters"));
            Utils.ApplyStyleToParagraph(document, "Heading6", "Heading6", ramsBody);

            var rInvokePara = body.AppendChild(new Paragraph());
            var rInvokeRun = rInvokePara.AppendChild(new Run());
            rInvokeRun.AppendChild(GetParamsTable(document, i.Response.OutputParameters));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rInvokePara);
            Utils.InsertSpacer(document);
        }

        private static void BuildInvocationTable(WordprocessingDocument document, Invocation invocation)
        {
            _log.Info("Printing InvocationTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text(invocation.Name));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", invokes);

            var idBody = body.AppendChild(new Paragraph());
            var idRun = idBody.AppendChild(new Run());
            idRun.AppendChild(new Text("Id: " + invocation.Id));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

            var iDescription = body.AppendChild(new Paragraph());
            var iRun = iDescription.AppendChild(new Run());
            iRun.AppendChild(new Text(invocation.Description));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", iDescription);

            var requestBody = body.AppendChild(new Paragraph());
            var requestRun = requestBody.AppendChild(new Run());
            requestRun.AppendChild(new Text("Request"));
            Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", requestBody);

            var reqBody = body.AppendChild(new Paragraph());
            var reqRun = reqBody.AppendChild(new Run());
            reqRun.AppendChild(new Text("Control Message: " + invocation.Request.ControlMessageName));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", reqBody);

            var dBody = body.AppendChild(new Paragraph());
            var dRun = dBody.AppendChild(new Run());
            dRun.AppendChild(new Text("Description: " + invocation.Request.Description));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", dBody);

            var paramsBody = body.AppendChild(new Paragraph());
            var paramsRun = paramsBody.AppendChild(new Run());
            paramsRun.AppendChild(new Text("Parameters"));
            Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", paramsBody);

            var invokePara = body.AppendChild(new Paragraph());
            var invokeRun = invokePara.AppendChild(new Run());
            invokeRun.AppendChild(GetParamsTable(document, invocation.Request.InputParameters));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", invokePara);

            //response
            var responseBody = body.AppendChild(new Paragraph());
            var responseRun = responseBody.AppendChild(new Run());
            responseRun.AppendChild(new Text("Response"));
            Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", responseBody);

            var resBody = body.AppendChild(new Paragraph());
            var resRun = resBody.AppendChild(new Run());
            resRun.AppendChild(new Text("Control Message: " + invocation.Response.ControlMessageName));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", resBody);

            var rBody = body.AppendChild(new Paragraph());
            var rRun = rBody.AppendChild(new Run());
            rRun.AppendChild(new Text("Description: " + invocation.Response.Description));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rBody);

            var ramsBody = body.AppendChild(new Paragraph());
            var ramsRun = ramsBody.AppendChild(new Run());
            ramsRun.AppendChild(new Text("Parameters"));
            Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", ramsBody);

            var rinvokePara = body.AppendChild(new Paragraph());
            var rinvokeRun = rinvokePara.AppendChild(new Run());
            rinvokeRun.AppendChild(GetParamsTable(document, invocation.Response.OutputParameters));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rinvokePara);
        }

        internal static void BuildInfluenceBindings(WordprocessingDocument document, IEnumerable<InfluenceBinding> influenceBindings, ArtifactType invocationFromType)
        {
            _log.Info("Printing InvocationsTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text("Influence Bindings"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", invokes);

            foreach (var i in influenceBindings)
            {
                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Influenced Id: " + i.InfluencedId));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var influencedName = Utils.GetNameForId(i.InfluencedId, ArtifactType.Behavior);
                var indBody = body.AppendChild(new Paragraph());
                var indRun = indBody.AppendChild(new Run());
                indRun.AppendChild(new Text("Influenced Name: " + influencedName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", indBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Influenced Invocation Id: " + i.InfluencedInvocationId));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                var requestBody = body.AppendChild(new Paragraph());
                var requestRun = requestBody.AppendChild(new Run());
                requestRun.AppendChild(new Text("Influence Type: " + i.InfluenceType));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", requestBody);

                var reqBody = body.AppendChild(new Paragraph());
                var reqRun = reqBody.AppendChild(new Run());
                reqRun.AppendChild(new Text("Influencing Invocation: "));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", reqBody, JustificationValues.Center);

                BuildInvocationTable(document, i.InfluencingInvocation);

                var resBody = body.AppendChild(new Paragraph());
                var resRun = resBody.AppendChild(new Run());
                resRun.AppendChild(new Text("Influenced Invocation: "));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", resBody, JustificationValues.Center);

                if (i.InfluencedInvocation == null)
                    i.InfluencedInvocation = Utils.GetInvocationById(i.InfluencedId, i.InfluencedInvocationId, invocationFromType);
                BuildInvocationTable(document, i.InfluencedInvocation);
            }
        }

        internal static void AddTaxonomyInfo(WordprocessingDocument document, TaxonomyVersion version)
        {
            _log.Info("Printing Taxonomy Info: " + version.Id );

            var body = document.MainDocumentPart.Document.Body;
            
            var title = body.AppendChild(new Paragraph());
            var titleRun = title.AppendChild(new Run());
            titleRun.AppendChild(new Text("Token Taxonomy Framework") { Space = SpaceProcessingModeValues.Preserve });
           
            Utils.ApplyStyleToParagraph(document, "Title", "Title", title, JustificationValues.Center);
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }
        
        internal static void AddSectionPage(WordprocessingDocument document, string sectionName)
        {
            _log.Info("Adding Section: " + sectionName );

            var body = document.MainDocumentPart.Document.Body;
            
            var title = body.AppendChild(new Paragraph());
            var titleRun = title.AppendChild(new Run());
            titleRun.AppendChild(new Text(sectionName) { Space = SpaceProcessingModeValues.Preserve });
           
            Utils.ApplyStyleToParagraph(document, "Title", "Title", title, JustificationValues.Center);
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }
            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }
        
        internal static void BuildPropertiesTable(WordprocessingDocument document, IEnumerable<Property> properties,
            bool book)
        {
            _log.Info("Printing Properties Table");
            var body = document.MainDocumentPart.Document.Body;
            var propertyPara = body.AppendChild(new Paragraph());
            var ivRun = propertyPara.AppendChild(new Run());
            ivRun.AppendChild(new Text("Properties"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", propertyPara);

            foreach (var p in properties)
            {
                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text("Name: " + p.Name));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", exBody);

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Value Description: " + p.ValueDescription));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Template Value: " + p.TemplateValue));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                BuildInvocationsTable(document, p.PropertyInvocations);
                if(p.Properties.Any())
                    BuildPropertiesTable(document, p.Properties, book);
            }

            if (!book) return;
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", pageBreak);
        }

        internal static void BuildPropertySpecificationTable(WordprocessingDocument document, IEnumerable<PropertySpecification> properties)
        {
            _log.Info("Printing Property Specifications Table");
            var body = document.MainDocumentPart.Document.Body;
            var propertyPara = body.AppendChild(new Paragraph());
            var ivRun = propertyPara.AppendChild(new Run());
            ivRun.AppendChild(new Text("Properties"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", propertyPara);

            foreach (var p in properties)
            {
                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text("Property Name: " + p.Name));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", exBody);

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Property Value Description: " + p.ValueDescription));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Template Value is set to: " + p.TemplateValue));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                BuildInvocationBindingsTable(document, p.PropertyInvocations, p.Name);
                if (p.Properties == null) continue;
                foreach (var pp in p.Properties)
                {
                    BuildPropertySpecificationTable(document, p.Properties);
                }
            }
        }
        
        public static Table GetParamsTable(WordprocessingDocument document, IEnumerable<InvocationParameter> parameters)
        {
            //dependencies table should be the 4th table
            var paramsTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Name"))));
            var width = new TableCellWidth {Type = TableWidthUnitValues.Pct, Width = "35"};
            name1.Append(new TableCellProperties(
                width));
            description1.Append(new Paragraph(new Run(new Text("Value"))));
            var element = new TableCellWidth {Type = TableWidthUnitValues.Pct, Width = "65"};
            description1.Append(new TableCellProperties(
                element));
            tr1.Append(name1);
            tr1.Append(description1);
            paramsTable.Append(tr1);
            foreach (var d in parameters)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(d.Name))));
                description.Append(new Paragraph(new Run(new Text(d.ValueDescription))));
         
                tr.Append(name);
                tr.Append(description);
                paramsTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", paramsTable);
            return paramsTable;
        }
    }
}
