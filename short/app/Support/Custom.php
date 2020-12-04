<?php

namespace App\Support;

use Storage;

class Custom
{
    /**
     * Get's the app icon.
     *
     * @return string
     */
    public static function getIcon()
    {
        $icon = setting('app_icon');
        switch ($icon) {
            case null:
            case 'default':
                return asset('img/app/icon.png');
                break;
            default:
                if (Storage::disk('public')->exists($icon)) {
                    return url(Storage::disk('public')->url($icon));
                } else {
                    return asset('img/app/icon.png');
                }
        }
    }

    /**
     * Get's the app background.
     *
     * @return string
     */
    public static function getBackground()
    {
        $background = setting('app_background');
        switch ($background) {
            case null:
            case 'default':
                return asset('img/app/background.jpg');
                break;
            default:
                if (Storage::disk('public')->exists($background)) {
                    return url(Storage::disk('public')->url($background));
                } else {
                    return asset('img/app/background.jpg');
                }
        }
    }
}
