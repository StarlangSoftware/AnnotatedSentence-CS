using System.Drawing;
using System.Globalization;
using Corpus;
using DependencyParser.Universal;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NamedEntityRecognition;
using PropBank;

namespace AnnotatedSentence
{
    public class AnnotatedWord : Word
    {
        private MorphologicalParse _parse;
        private MetamorphicParse _metamorphicParse;
        private string _semantic;
        private NamedEntityType _namedEntityType;
        private Argument _argument;
        private UniversalDependencyRelation _universalDependency;
        private string _shallowParse;
        private Rectangle _area;
        private bool _selected;

        /**
         * <summary> Constructor for the {@link AnnotatedWord} class. Gets the word with its annotation layers as input and sets the
         * corresponding layers.</summary>
         * <param name="word">Input word with annotation layers</param>
         */
        public AnnotatedWord(string word)
        {
            var splitLayers = word.Split('[', '{', '}', ']');
            foreach (var layer in splitLayers)
            {
                if (layer == "")
                    continue;
                if (!layer.Contains("="))
                {
                    name = layer;
                    continue;
                }

                var layerType = layer.Substring(0, layer.IndexOf("="));
                var layerValue = layer.Substring(layer.IndexOf("=") + 1);
                switch (layerType)
                {
                    case "turkish":
                        name = layerValue;
                        break;
                    case "morphologicalAnalysis":
                        _parse = new MorphologicalParse(layerValue);
                        break;
                    case "metaMorphemes":
                        _metamorphicParse = new MetamorphicParse(layerValue);
                        break;
                    case "semantics":
                        _semantic = layerValue;
                        break;
                    case "namedEntity":
                        _namedEntityType = NamedEntityTypeStatic.GetNamedEntityType(layerValue);
                        break;
                    case "propbank":
                        _argument = new Argument(layerValue);
                        break;
                    case "shallowParse":
                        _shallowParse = layerValue;
                        break;
                    case "universalDependency":
                    {
                        var values = layerValue.Split("$");
                        _universalDependency = new UniversalDependencyRelation(int.Parse(values[0]), values[1]);
                        break;
                    }
                }
            }
        }

        /**
         * <summary> Converts an {@link AnnotatedWord} to string. For each annotation layer, the method puts a left brace, layer name,
         * equal sign and layer value finishing with right brace.</summary>
         * <returns>string form of the {@link AnnotatedWord}.</returns>
         */
        public override string ToString()
        {
            var result = "{turkish=" + name + "}";
            if (_parse != null)
            {
                result = result + "{morphologicalAnalysis=" + _parse + "}";
            }

            if (_metamorphicParse != null)
            {
                result = result + "{metaMorphemes=" + _metamorphicParse + "}";
            }

            if (_semantic != null)
            {
                result = result + "{semantics=" + _semantic + "}";
            }

            result = result + "{namedEntity=" + _namedEntityType + "}";

            if (_argument != null)
            {
                result = result + "{propbank=" + _argument + "}";
            }

            if (_shallowParse != null)
            {
                result = result + "{shallowParse=" + _shallowParse + "}";
            }

            if (_universalDependency != null)
            {
                result = result + "{universalDependency=" + _universalDependency.To() + "$" +
                         _universalDependency + "}";
            }

            return result;
        }

        /**
         * <summary> Another constructor for {@link AnnotatedWord}. Gets the word and a namedEntityType and sets two layers.</summary>
         * <param name="name">Lemma of the word.</param>
         * <param name="namedEntityType">Named entity of the word.</param>
         */
        public AnnotatedWord(string name, NamedEntityType namedEntityType) : base(name)
        {
            this._namedEntityType = namedEntityType;
            _parse = null;
            _metamorphicParse = null;
            _semantic = null;
            _argument = new Argument("NONE", null);
            _shallowParse = null;
            _universalDependency = null;
        }

        /**
         * <summary> Another constructor for {@link AnnotatedWord}. Gets the word and morphological parse and sets two layers.</summary>
         * <param name="name">Lemma of the word.</param>
         * <param name="parse">Morphological parse of the word.</param>
         */
        public AnnotatedWord(string name, MorphologicalParse parse) : base(name)
        {
            this._parse = parse;
            this._namedEntityType = NamedEntityType.NONE;
            _argument = new Argument("NONE", null);
            _metamorphicParse = null;
            _semantic = null;
            _shallowParse = null;
            _universalDependency = null;
        }

        /**
         * <summary> Another constructor for {@link AnnotatedWord}. Gets the word and morphological parse and sets two layers.</summary>
         * <param name="name">Lemma of the word.</param>
         * <param name="parse">Morphological parse of the word.</param>
         */
        public AnnotatedWord(string name, FsmParse parse) : base(name)
        {
            this._parse = parse;
            this._namedEntityType = NamedEntityType.NONE;
            _argument = new Argument("NONE", null);
            SetMetamorphicParse(parse.WithList());
            _semantic = null;
            _shallowParse = null;
            _universalDependency = null;
        }

        /**
         * <summary> Returns the value of a given layer.</summary>
         * <param name="viewLayerType">Layer for which the value questioned.</param>
         * <returns>The value of the given layer.</returns>
         */
        public string GetLayerInfo(ViewLayerType viewLayerType)
        {
            switch (viewLayerType)
            {
                case ViewLayerType.INFLECTIONAL_GROUP:
                    if (_parse != null)
                    {
                        return _parse.ToString();
                    }

                    break;
                case ViewLayerType.META_MORPHEME:
                    if (_metamorphicParse != null)
                    {
                        return _metamorphicParse.ToString();
                    }

                    break;
                case ViewLayerType.SEMANTICS:
                    return _semantic;
                case ViewLayerType.NER:
                    return _namedEntityType.ToString();
                case ViewLayerType.SHALLOW_PARSE:
                    return _shallowParse;
                case ViewLayerType.TURKISH_WORD:
                    return name;
                case ViewLayerType.PROPBANK:
                    if (_argument != null)
                    {
                        return _argument.ToString();
                    }

                    break;
                case ViewLayerType.DEPENDENCY:
                    if (_universalDependency != null)
                    {
                        return _universalDependency.To() + "$" + _universalDependency;
                    }

                    break;
            }

            return null;
        }

        /**
         * <summary> Returns the morphological parse layer of the word.</summary>
         * <returns>The morphological parse of the word.</returns>
         */
        public MorphologicalParse GetParse()
        {
            return _parse;
        }

        /**
         * <summary> Sets the morphological parse layer of the word.</summary>
         * <param name="parseString">The new morphological parse of the word in string form.</param>
         */
        public void SetParse(string parseString)
        {
            if (parseString != null)
            {
                _parse = new MorphologicalParse(parseString);
            }
            else
            {
                _parse = null;
            }
        }

        /**
         * <summary> Returns the metamorphic parse layer of the word.</summary>
         * <returns>The metamorphic parse of the word.</returns>
         */
        public MetamorphicParse GetMetamorphicParse()
        {
            return _metamorphicParse;
        }

        /**
         * <summary> Sets the metamorphic parse layer of the word.</summary>
         * <param name="parseString">The new metamorphic parse of the word in string form.</param>
         */
        public void SetMetamorphicParse(string parseString)
        {
            _metamorphicParse = new MetamorphicParse(parseString);
        }

        /**
         * <summary> Returns the semantic layer of the word.</summary>
         * <returns>Sense id of the word.</returns>
         */
        public string GetSemantic()
        {
            return _semantic;
        }

        /**
         * <summary> Sets the semantic layer of the word.</summary>
         * <param name="semantic">New sense id of the word.</param>
         */
        public void SetSemantic(string semantic)
        {
            this._semantic = semantic;
        }

        /**
         * <summary> Returns the named entity layer of the word.</summary>
         * <returns>Named entity tag of the word.</returns>
         */
        public NamedEntityType GetNamedEntityType()
        {
            return _namedEntityType;
        }

        /**
         * <summary> Sets the named entity layer of the word.</summary>
         * <param name="namedEntity">New named entity tag of the word.</param>
         */
        public void SetNamedEntityType(string namedEntity)
        {
            if (namedEntity != null)
            {
                _namedEntityType = NamedEntityTypeStatic.GetNamedEntityType(namedEntity);
            }
            else
            {
                _namedEntityType = NamedEntityType.NONE;
            }
        }

        /**
         * <summary> Returns the semantic role layer of the word.</summary>
         * <returns>Semantic role tag of the word.</returns>
         */
        public Argument GetArgument()
        {
            return _argument;
        }

        /**
         * <summary> Sets the semantic role layer of the word.</summary>
         * <param name="argument">New semantic role tag of the word.</param>
         */
        public void SetArgument(string argument)
        {
            if (argument != null)
            {
                this._argument = new Argument(argument);
            }
            else
            {
                this._argument = null;
            }
        }

        /**
         * <summary> Returns the shallow parse layer of the word.</summary>
         * <returns>Shallow parse tag of the word.</returns>
         */
        public string GetShallowParse()
        {
            return _shallowParse;
        }

        /**
         * <summary> Sets the shallow parse layer of the word.</summary>
         * <param name="parse">New shallow parse tag of the word.</param>
         */
        public void SetShallowParse(string parse)
        {
            _shallowParse = parse;
        }

        /**
         * <summary> Returns the universal dependency layer of the word.</summary>
         * <returns>Universal dependency relation of the word.</returns>
         */
        public UniversalDependencyRelation GetUniversalDependency()
        {
            return _universalDependency;
        }

        /**
         * <summary> Sets the universal dependency layer of the word.</summary>
         * <param name="to">Word related to.</param>
         * <param name="dependencyType">type of dependency the word is related to.</param>
         */
        public void SetUniversalDependency(int to, string dependencyType)
        {
            _universalDependency = new UniversalDependencyRelation(to, dependencyType);
        }

        public string GetFormattedString(WordFormat format)
        {
            switch (format)
            {
                case WordFormat.SURFACE:
                    return name;
                default:
                    return name;
            }
        }

        /**
         * <summary> Accessor method for the area attribute.</summary>
         * <returns>Area attribute.</returns>
         */
        public Rectangle GetArea()
        {
            return _area;
        }

        /**
         * <summary> Mutator method for the area attribute.</summary>
         * <param name="area">New area attribute.</param>
         */
        public void SetArea(Rectangle area)
        {
            this._area = area;
        }

        /**
         * <summary> Accessor method for the selected attribute.</summary>
         * <returns>Selected attribute value.</returns>
         */
        public bool IsSelected()
        {
            return _selected;
        }

        /**
         * <summary> Mutator method for the selected attribute.</summary>
         * <param name="selected">New value for the selected attribute.</param>
         */
        public void SetSelected(bool selected)
        {
            this._selected = selected;
        }

        public void CheckGazetteer(Gazetteer gazetteer)
        {
            var wordLowercase = name.ToLower(new CultureInfo("tr"));
            if (gazetteer.Contains(wordLowercase) && _parse.ContainsTag(MorphologicalTag.PROPERNOUN))
            {
                SetNamedEntityType(gazetteer.GetName());
            }

            if (wordLowercase.Contains("'") && gazetteer.Contains(
                                                wordLowercase.Substring(0, wordLowercase.IndexOf("'")))
                                            && _parse.ContainsTag(MorphologicalTag.PROPERNOUN))
            {
                SetNamedEntityType(gazetteer.GetName());
            }
        }
    }
}