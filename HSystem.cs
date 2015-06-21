﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HazeronProspector
{
    class HSystem
    {
        protected Dictionary<string, CelestialBody> _celestialBodies = new Dictionary<string, CelestialBody>();
        public Dictionary<string, CelestialBody> CelestialBodies
        {
            get { return _celestialBodies; }
        }

        protected Sector _hostSector;
        public Sector HostSector
        {
            get { return _hostSector; }
            set { _hostSector = value; }
        }

        protected string _name;
        public string Name
        {
            get { return _name; }
        }

        protected string _id;
        public string ID
        {
            get { return _id; }
        }

        protected Coordinate _coord;
        public Coordinate Coord
        {
            get { return _coord; }
        }

        protected string _eod;
        public string EOD
        {
            get { return _eod; }
        }

        protected bool _initialized = false;
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        protected List<HSystem> _wormholeLinks = new List<HSystem>();
        public List<HSystem> WormholeLinks
        {
            get { return _wormholeLinks; }
        }

        public HSystem(string id, string name, string x, string y, string z, string eod)
        {
            _id = id;
            _name = name;
            double nX = Convert.ToDouble(x, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            double nY = Convert.ToDouble(y, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            double nZ = Convert.ToDouble(z, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            _coord = new Coordinate(nX, nY, nZ);
            _eod = eod;
        }

        public void AddCelestialBody(CelestialBody celestialBody)
        {
            _celestialBodies.Add(celestialBody.ID, celestialBody);
            celestialBody.HostSystem = this;
        }

        public void AddWormholeLink(HSystem destination)
        {
            if (!_wormholeLinks.Contains(destination))
                _wormholeLinks.Add(destination);
        }

        public Dictionary<ResourceType, Resource> BestResources()
        {
            List<Dictionary<ResourceType, Resource>> resourceLists = new List<Dictionary<ResourceType, Resource>>();
            foreach (CelestialBody body in _celestialBodies.Values)
            {
                foreach (Zone zone in body.ResourceZones)
                {
                    resourceLists.Add(body.BestResources());
                }
            }
            return resourceLists.SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(k => k.Key, v => v.Aggregate((i, j) => i.Quality > j.Quality ? i : j));
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
