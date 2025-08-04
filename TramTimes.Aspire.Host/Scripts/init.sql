create database southyorkshire;
\connect southyorkshire;

create or replace function get_from_point(
    originLongitude real,
    originLatitude real,
    destinationLongitude real,
    destinationLatitude real
) returns real AS $$

declare
    angle real;
    deltaLatitude real;
    deltaLongitude real;
    distance real;
    
    a real;
    b real;
    x real;
    y real;
    
begin
    deltaLatitude := radians(destinationLatitude - originLatitude);
    deltaLongitude := radians(destinationLongitude - originLongitude);
    
    a := sin(deltaLatitude / 2) *
         sin(deltaLatitude / 2);
    b := cos(radians(originLatitude)) *
         cos(radians(destinationLatitude)) *
         sin(deltaLongitude / 2) *
         sin(deltaLongitude / 2);
    
    y := sqrt(a + b);
    x := sqrt(1 - (a + b));
    
    angle := 2 * atan2(y, x);
    distance := angle * 6371;
    
    return distance;
end;

$$ language plpgsql;