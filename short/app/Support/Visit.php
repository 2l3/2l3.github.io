<?php

namespace App\Support;

/**
 * Class Agent
 * @package App\Support
 */
class Visit
{
    /**
     * @param $agent
     * @return string
     */
    public static function getDevice($agent)
    {
        if ($agent->isMobile()) {
            return 'mobile';
        } else {
            if ($agent->isTablet()) {
                return 'tablet';
            } else {
                return 'desktop';
            }
        }
    }
}
