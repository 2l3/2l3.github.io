<?php

namespace App\Support;

use Str;

class Chart
{
    /**
     * @param $browser
     * @return string
     */
    public static function browserCode($browser)
    {
        return Str::slug($browser, '_');
    }

    /**
     * @param $browser
     * @return string
     */
    public static function browserName($browser)
    {
        switch ($browser) {
            case 'ie':
                return __('Internet explorer');
                break;
            default:
                return Str::ucfirst($browser);
                break;
        }
    }

    /**
     * @param $browser
     * @return string
     */
    public static function browserColor($browser)
    {
        switch ($browser) {
            case 'chrome':
                return '#f14038';
                break;
            case 'edge':
                return '#1496f8';
                break;
            case 'firefox':
            case 'mozilla':
                return '#ff941d';
                break;
            case 'ie':
                return '#1ebbee';
                break;
            case 'netscape':
                return '#007f7f';
                break;
            case 'opera':
            case 'opera mini':
                return '#f4162b';
                break;
            case 'safari':
                return '#326deb';
                break;
            default:
                return '#3a1143';
                break;
        }
    }

    /**
     * @param $platform
     * @return string
     */
    public static function platformName($platform)
    {
        switch ($platform) {
            case 'os-x':
                return __('OS X');
                break;
            case 'ios':
                return __('IOS');
                break;
            case 'androidos':
                return __('Android');
                break;
            case 'chromeos':
                return __('Chrome OS');
                break;
            default:
                return Str::ucfirst($platform);
                break;
        }
    }

    /**
     * @param $platform
     * @return string
     */
    public static function platformColor($platform)
    {
        switch ($platform) {
            case 'androidos':
                return '#a5c639';
                break;
            case 'blackberry':
                return '#010101';
                break;
            case 'chromeos':
                return '#f14038';
                break;
            case 'debian':
                return '#a30333';
                break;
            case 'ios':
                return '#999999';
                break;
            case 'os-x':
                return '#78a5c6';
                break;
            case 'ubuntu':
                return '#dc4915';
                break;
            case 'windows':
                return '#00adef';
                break;
            default:
                return '#3a1143';
                break;
        }
    }

    /**
     * @param $referrer
     * @return string
     */
    public static function referrerLabel($referrer)
    {
        switch ($referrer) {
            case null:
            case '';
                return __('Direct');
                break;
            default:
                return $referrer;
                break;
        }
    }
}
