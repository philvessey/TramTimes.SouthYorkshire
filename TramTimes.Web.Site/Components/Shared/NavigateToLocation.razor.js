// noinspection all

export function getPosition(helper, longitude, latitude) {
    navigator.geolocation.getCurrentPosition(
        function(position) {
            helper.invokeMethodAsync("OnPositionAsync", position.coords.longitude, position.coords.latitude);
        },
        function() {
            helper.invokeMethodAsync("OnPositionAsync", longitude, latitude);
        }
    );
}

export function writeConsole(message) {
    console.log(message);
}