// noinspection all

export function getPosition(helper, longitude, latitude) {
    navigator.geolocation.getCurrentPosition(
        function(position) {
            helper.invokeMethodAsync("OnPosition", position.coords.longitude, position.coords.latitude);
        },
        function() {
            helper.invokeMethodAsync("OnPosition", longitude, latitude);
        }
    );
}