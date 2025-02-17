using System.Drawing;
using System.Globalization;
using Corpus;
using DependencyParser.Universal;
using Dictionary.Dictionary;
using FrameNet;
using MorphologicalAnalysis;
using NamedEntityRecognition;
using PropBank;
using SentiNet;

namespace AnnotatedSentence
{
    public class AnnotatedWord : Word
    {
        /**
     * In order to add another layer, do the following:
     * 1. Select a name for the layer.
     * 2. Add a new constant to ViewLayerType.
     * 3. Add private attribute.
     * 4. Add an if-else to the constructor, where you set the private attribute with the layer name.
     * 5. Update toString method.
     * 6. Add initial value to the private attribute in other constructors.
     * 7. Update getLayerInfo.
     * 8. Add getter and setter methods.
     */
        private MorphologicalParse _parse;

        private MetamorphicParse _metamorphicParse;
        private string _semantic;
        private NamedEntityType? _namedEntityType;
        private ArgumentList _argumentList;
        private FrameElementList _frameElementList;
        private UniversalDependencyRelation _universalDependency;
        private Slot _slot;
        private string _shallowParse;
        private PolarityType? _polarity;
        private Rectangle _area;
        private bool _selected;
        private string _ccg;
        private string _posTag;
        private Language _language;

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
                    case "english":
                    case "persian":
                        name = layerValue;
                        _language = GetLanguageFromString(layerType);
                        break;
                    case "morphologicalAnalysis":
                    case "morphologicalanalysis":
                        _parse = new MorphologicalParse(layerValue);
                        break;
                    case "metaMorphemes":
                    case "metamorphemes":
                        _metamorphicParse = new MetamorphicParse(layerValue);
                        break;
                    case "semantics":
                        _semantic = layerValue;
                        break;
                    case "namedEntity":
                    case "namedentity":
                        _namedEntityType = NamedEntityTypeStatic.GetNamedEntityType(layerValue);
                        break;
                    case "propBank":
                    case "propbank":
                        _argumentList = new ArgumentList(layerValue);
                        break;
                    case "framenet":
                    case "frameNet":
                        _frameElementList = new FrameElementList(layerValue);
                        break;
                    case "slot":
                        _slot = new Slot(layerValue);
                        break;
                    case "polarity":
                        SetPolarity(layerValue);
                        break;
                    case "shallowParse":
                    case "shallowparse":
                        _shallowParse = layerValue;
                        break;
                    case "ccg":
                        _ccg = layerValue;
                        break;
                    case "posTag":
                        _posTag = layerValue;
                        break;
                    case "universalDependency":
                    case "universaldependency":
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
            var result = "";
            switch (_language)
            {
                case Language.TURKISH:
                    result = "{turkish=" + name + "}";
                    break;
                case Language.ENGLISH:
                    result = "{english=" + name + "}";
                    break;
                case Language.PERSIAN:
                    result = "{persian=" + name + "}";
                    break;
            }

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

            if (_namedEntityType != null)
            {
                result = result + "{namedEntity=" + _namedEntityType + "}";
            }

            if (_argumentList != null)
            {
                result = result + "{propbank=" + _argumentList + "}";
            }

            if (_frameElementList != null)
            {
                result = result + "{framenet=" + _frameElementList + "}";
            }

            if (_slot != null)
            {
                result = result + "{slot=" + _slot + "}";
            }

            if (_polarity != null)
            {
                result = result + "{polarity=" + GetPolarityString() + "}";
            }

            if (_shallowParse != null)
            {
                result = result + "{shallowParse=" + _shallowParse + "}";
            }

            if (_ccg != null)
            {
                result = result + "{ccg=" + _ccg + "}";
            }

            if (_posTag != null)
            {
                result = result + "{posTag=" + _posTag + "}";
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
            _argumentList = null;
            _shallowParse = null;
            _universalDependency = null;
            _frameElementList = null;
            _slot = null;
            _polarity = PolarityType.NEUTRAL;
            _ccg = null;
            _posTag = null;
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
            _argumentList = null;
            _metamorphicParse = null;
            _semantic = null;
            _shallowParse = null;
            _universalDependency = null;
            _frameElementList = null;
            _slot = null;
            _polarity = PolarityType.NEUTRAL;
            _ccg = null;
            _posTag = null;
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
            _argumentList = null;
            SetMetamorphicParse(parse.WithList());
            _semantic = null;
            _shallowParse = null;
            _universalDependency = null;
            _frameElementList = null;
            _slot = null;
            _polarity = PolarityType.NEUTRAL;
            _ccg = null;
            _posTag = null;
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
                    if (_argumentList != null)
                    {
                        return _argumentList.ToString();
                    }

                    break;
                case ViewLayerType.FRAMENET:
                    if (_frameElementList != null)
                    {
                        return _frameElementList.ToString();
                    }

                    break;
                case ViewLayerType.SLOT:
                    if (_slot != null)
                    {
                        return _slot.ToString();
                    }

                    break;
                case ViewLayerType.POLARITY:
                    return GetPolarityString();
                case ViewLayerType.CCG:
                    return _ccg;
                case ViewLayerType.POS_TAG:
                    return _posTag;
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
            return (NamedEntityType) _namedEntityType;
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
        public ArgumentList GetArgumentList()
        {
            return _argumentList;
        }

        /**
         * <summary> Sets the semantic role layer of the word.</summary>
         * <param name="argumentList">New semantic role tag of the word.</param>
         */
        public void SetArgumentList(string argumentList)
        {
            if (argumentList != null)
            {
                this._argumentList = new ArgumentList(argumentList);
            }
            else
            {
                this._argumentList = null;
            }
        }

        /**
         * <summary> Returns the frame element layer of the word.</summary>
         * <returns>Frame element tag of the word.</returns>
         */
        public FrameElementList GetFrameElementList()
        {
            return _frameElementList;
        }

        /**
         * <summary> Sets the frame element layer of the word.</summary>
         * <param name="frameElementList">New frame element tag of the word.</param>
         */
        public void SetFrameElementList(string frameElementList)
        {
            if (frameElementList != null)
            {
                this._frameElementList = new FrameElementList(frameElementList);
            }
            else
            {
                this._frameElementList = null;
            }
        }

        /**
         * <summary> Returns the slot filling layer of the word.</summary>
         * <returns>Slot tag of the word.</returns>
         */
        public Slot GetSlot()
        {
            return _slot;
        }

        /**
         * <summary> Sets the slot filling layer of the word.</summary>
         * <param name="slot">New slot tag of the word.</param>
         */
        public void SetSlot(string slot)
        {
            if (slot != null)
            {
                this._slot = new Slot(slot);
            }
            else
            {
                this._slot = null;
            }
        }

        /**
         * <summary>Returns the polarity layer of the word.</summary>
         * <returns>Polarity type of the word.</returns>
         */
        public PolarityType GetPolarity()
        {
            return (PolarityType) _polarity;
        }

        /**
         * <summary> Sets the polarity layer of the word.</summary>
         * <param name="polarity">New polarity of the word.</param>
         */
        public void SetPolarity(string polarity)
        {
            if (polarity != null)
            {
                switch (polarity.ToLower())
                {
                    case "positive":
                    case "pos":
                        _polarity = PolarityType.POSITIVE;
                        break;
                    case "negative":
                    case "neg":
                        _polarity = PolarityType.NEGATIVE;
                        break;
                    default:
                        _polarity = PolarityType.NEUTRAL;
                        break;
                }
            }
            else
            {
                _polarity = PolarityType.NEUTRAL;
            }
        }

        /// <summary>
        /// Returns the polarity layer of the word.
        /// </summary>
        /// <returns>Polarity string of the word.</returns>
        public string GetPolarityString()
        {
            switch (_polarity)
            {
                case PolarityType.POSITIVE:
                    return "positive";
                case PolarityType.NEGATIVE:
                    return "negative";
                case PolarityType.NEUTRAL:
                    return "neutral";
                default:
                    return "neutral";
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
         * <summary> Returns the ccg layer of the word.</summary>
         * <returns> Ccg tag of the word.</returns>
         */
        public string GetCcg()
        {
            return _ccg;
        }

        /**
         * <summary> Sets the ccg layer of the word.</summary>
         * <param name="ccg">New ccg tag of the word.</param>
         */
        public void SetCcg(string ccg)
        {
            _ccg = ccg;
        }

        /**
         * <summary> Returns the posTag layer of the word.</summary>
         * <returns> Pos tag of the word.</returns>
         */
        public string GetPosTag()
        {
            return _posTag;
        }

        /**
         * <summary> Sets the posTag layer of the word.</summary>
         * <param name="posTag">New pos tag of the word.</param>
         */
        public void SetPosTag(string posTag)
        {
            _posTag = posTag;
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

        /// <summary>
        /// Returns the connlu format string for this word. Adds surface form, root, universal pos tag, features, and
        /// universal dependency information.
        /// </summary>
        /// <param name="sentenceLength">Number of words in the sentence.</param>
        /// <returns>The connlu format string for this word.</returns>
        public string GetUniversalDependencyFormat(int sentenceLength)
        {
            if (_parse != null)
            {
                var uPos = _parse.GetUniversalDependencyPos();
                var result = name + "\t" + _parse.GetWord().GetName() + "\t" + uPos +
                             "\t_\t";
                var features = _parse.GetUniversalDependencyFeatures(uPos);
                if (features.Count == 0)
                {
                    result += "_";
                }
                else
                {
                    var first = true;
                    foreach (var feature in features)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            result += "|";
                        }

                        result += feature;
                    }
                }

                result += "\t";
                if (_universalDependency != null && _universalDependency.To() <= sentenceLength)
                {
                    result += _universalDependency.To() + "\t" + _universalDependency.ToString().ToLower() + "\t";
                }
                else
                {
                    result += "_\t_\t";
                }

                result += "_\t_";
                return result;
            }

            return name + "\t" + name + "\t_\t_\t_\t_\t_\t_\t_";
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

        /// <summary>
        /// Checks the gazetteer and sets the named entity tag accordingly.
        /// </summary>
        /// <param name="gazetteer">Gazetteer used to set named entity tag.</param>
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

        /**
        * Converts a language string to language.
        * @param languageString String defining the language name.
        * @return Language corresponding to the languageString.
        */
        private Language GetLanguageFromString(string languageString)
        {
            switch (languageString)
            {
                case "turkish":
                case "Turkish":
                    return Language.TURKISH;
                case "english":
                case "English":
                    return Language.ENGLISH;
                case "persian":
                case "Persian":
                    return Language.PERSIAN;
            }

            return Language.TURKISH;
        }

        /**
        * Returns the language of the word.
        * @return The language of the word.
        */
        public Language GetLanguage()
        {
            return _language;
        }
    }
}