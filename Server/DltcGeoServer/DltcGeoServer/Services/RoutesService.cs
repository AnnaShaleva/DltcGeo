using DltcGeoServer.Models;
using Itinero;
using Itinero.Exceptions;
using Itinero.IO.Osm;
using Itinero.Profiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DltcGeoServer.Services
{
    public class RoutesService
    {
        private readonly RouterDb _routerDb;
        private readonly Router _router;

        public RoutesService()
        {
            _routerDb = new RouterDb();

            var stream = new FileStream("/app/LO.pbf", FileMode.Open);
            {
                try
                {
                    _routerDb.LoadOsmData(stream, new[]
                    {
                        Itinero.Osm.Vehicles.Vehicle.Car,
                        Itinero.Osm.Vehicles.Vehicle.Pedestrian
                    });
                    _router = new Router(_routerDb);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public IEnumerable<Point> GetPath(Point start, Point end, List<Profile> profiles)
        {
            Route resultRoute = null;

            foreach (var profile in profiles)
            {
                try
                {
                    var route = _router.Calculate(profile, (float)start.Latitude, (float)start.Longitude, (float)end.Latitude, (float)end.Longitude);
                    if (resultRoute == null || route.TotalDistance < resultRoute.TotalDistance)
                        resultRoute = route;
                }
                catch (ResolveFailedException e)
                {
                    Debug.WriteLine($"Выкинута точка {end.Latitude} {end.Longitude}");
                }
                catch (RouteNotFoundException e)
                {
                    Debug.WriteLine($"Путь не найден между точками: ({start.Latitude} {start.Longitude}) и ({end.Latitude} {end.Longitude})");
                }
            }

            return resultRoute
                ?.Shape
                ?.Select(p => new Point
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                });
        }

        public IEnumerable<Point> GetPathForGroup(List<Point> points, List<Profile> profiles)
        {
            Route resultRoute = null;

            foreach (var profile in profiles)
            {
                try
                {
                    var route = _router.Calculate(profile, points.Select(p => new Itinero.LocalGeo.Coordinate((float)p.Latitude, (float)p.Longitude)).ToArray());
                    if (resultRoute == null || resultRoute.TotalDistance < route.TotalDistance)
                        resultRoute = route;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Невозможность");
                }
            }

            return resultRoute
                ?.Shape
                ?.Select(p => new Point
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                });
        }
    }
}