module.exports = {
    SIGNOUT: '/auth/signout',
    SIGNIN: '/auth/signin',
    SIGNUP: '/auth/signup',

    NEW_DEVICE: '/api/users/me/devices',
    DEVICES: '/api/users/me/devices?fields=id,title,imei,description,softwareVersion',
    DEVICE: '/api/devices/{}?fields=id,title,imei,description,softwareVersion',
    DEVICE_CODE: '/api/devicecode/{}',
    DEVICE_CODE_LINK: '/trackers/{}/code',

    TRACKS: '/api/users/me/devices?fields=id,title,tracks.id,tracks.title,tracks.UniqCreatedTicks',
    TRACK_POINTS: '/api/tracks/{}/points?orderBy=Id desc&pageSize={}'
}