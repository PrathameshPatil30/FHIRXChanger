﻿using System.Collections.Generic;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using FhirXChangerService.Model;

namespace FhirXChangerService.Serialization
{
    public interface IParser<T> where T: Base
    {
        List<ParserError> Errors { get; set; }
        
        T FromXml(XElement element);
        T FromXml(XElement element, List<ParserError> errors);
    }

    public abstract class BaseParser<T> :IParser<T>  where T: Base
    {
        public Bundle Bundle { get; set; }

        public List<ParserError> Errors { get; set; } = new List<ParserError>();

        protected BaseParser()
        {
        }

        protected BaseParser(Bundle bundle)
        {
            Bundle = bundle;
        }

        public abstract T FromXml(XElement element);

        public virtual T FromXml(XElement element, List<ParserError> errors)
        {
            var result = FromXml(element);

            errors?.AddRange(Errors);

            return result;
        }

        public virtual TChild FromXml<TChild>(IParser<TChild> parser, XElement element) where TChild: Base
        {
            return parser.FromXml(element, Errors);
        }
    }
}
