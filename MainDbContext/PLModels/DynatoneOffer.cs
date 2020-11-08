using System;
using System.Collections.Generic;
using System.Text;

namespace DbCore.PLModels
{
    public class DynatoneOffer
    {
        public Guid Id { get; set; }
        public int? Kod { get; set; }
        public int? KodGruppy { get; set; }
        public string Naimenovanie { get; set; }
        public int? RRC { get; set; }
        public int? CenaDiler { get; set; }
        public int? KolichestvoDlyaOpta { get; set; }
        public string VidNomenklatury { get; set; }
        public string Brand { get; set; }
        public string ModelSModifikaciyey { get; set; }
        public string Articul { get; set; }
        public long? Barcode { get; set; }
        public string Model { get; set; }
        public string Modifikaciya { get; set; }
        public string KodSPrefixom { get; set; }
        public string StranaProishojdenia { get; set; }
        public float? Ves { get; set; }
        public float? Dlina { get; set; }
        public float? Shirina { get; set; }
        public float? Vysota { get; set; }

        public bool IsDescriptionChanged(DynatoneOffer oldOffer)
        {
            if (KodGruppy != oldOffer.KodGruppy ||
                Naimenovanie != oldOffer.Naimenovanie ||
                VidNomenklatury != oldOffer.VidNomenklatury ||
                Brand != oldOffer.Brand ||
                ModelSModifikaciyey != oldOffer.ModelSModifikaciyey ||
                Model != oldOffer.Model ||
                Modifikaciya != oldOffer.Modifikaciya ||
                StranaProishojdenia != oldOffer.StranaProishojdenia)
                return true;

            return false;
        }

        public bool IsPriceChanged(DynatoneOffer oldOffer)
        {
            if (CenaDiler != oldOffer.CenaDiler)
                return true;
            return false;
        }

        public void CopyFromWithoutId(DynatoneOffer offer)
        {
            Kod = offer.Kod;
            KodGruppy = offer.KodGruppy;
            Naimenovanie = offer.Naimenovanie;
            RRC = offer.RRC;
            CenaDiler = offer.CenaDiler;
            KolichestvoDlyaOpta = offer.KolichestvoDlyaOpta;
            VidNomenklatury = offer.VidNomenklatury;
            Brand = offer.Brand;
            ModelSModifikaciyey = offer.ModelSModifikaciyey;
            Articul = offer.Articul;
            Barcode = offer.Barcode;
            Model = offer.Model;
            Modifikaciya = offer.Modifikaciya;
            KodSPrefixom = offer.KodSPrefixom;
            StranaProishojdenia = offer.StranaProishojdenia;
            Ves = offer.Ves;
            Dlina = offer.Dlina;
            Shirina = offer.Shirina;
            Vysota = offer.Vysota;
        }
    }
}
