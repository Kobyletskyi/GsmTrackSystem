'use strict';

import {  } from './settings';

const fitBounds = (points, map) => {
    const bounds = new google.maps.LatLngBounds();
    points.forEach(p => bounds.extend({ lat: p.latitude, lng: p.longitude }));
    map.fitBounds(bounds);
}
const setPolyline = (points, map) => {
    return new google.maps.Polyline({
        path: points.map(p => ({ lat: p.latitude, lng: p.longitude })),
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2,
        map: map
    });
}
const setCircles = (points, map) => {
    points.forEach(p => new google.maps.Circle({
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.1,
        center: { lat: p.latitude, lng: p.longitude },
        radius: p.horizontalAccuracy / 2,
        map: map
    }));
}
const setDirectionMarkers = (points, map) => {
    points.forEach(p => new google.maps.Marker({
        icon: { path: 1, scale: 1.5, rotation: p.courseOverGround },
        position: { lat: p.latitude, lng: p.longitude },
        map: map
    }));
}

module.exports = {
    fitBounds,
    setCircles,
    setPolyline,
    setDirectionMarkers
}