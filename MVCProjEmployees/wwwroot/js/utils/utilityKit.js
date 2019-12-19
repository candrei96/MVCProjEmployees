export function calculateAge(birthday) {
    if (!(Object.prototype.toString.call(birthday) === '[object Date]')) return 0;

    let now = new Date();
    let diff = (now.getTime() - birthday.getTime()) / 1000;

    diff /= (60 * 60 * 24);

    return Math.abs(Math.floor(diff / 365.25));
}