create function get_from_point(
    origin_longitude real,
    origin_latitude real,
    destination_longitude real,
    destination_latitude real
) returns real as $$

declare
    angle real;
    delta_latitude real;
    delta_longitude real;
    distance real;

    a real;
    b real;
    x real;
    y real;

begin
    delta_latitude := radians(destination_latitude - origin_latitude);
    delta_longitude := radians(destination_longitude - origin_longitude);

    a := sin(delta_latitude / 2) *
         sin(delta_latitude / 2);
    b := cos(radians(origin_latitude)) *
         cos(radians(destination_latitude)) *
         sin(delta_longitude / 2) *
         sin(delta_longitude / 2);

    y := sqrt(a + b);
    x := sqrt(1 - (a + b));

    angle := 2 * atan2(y, x);
    distance := angle * 6371;

    return distance;
end;

$$ language plpgsql;