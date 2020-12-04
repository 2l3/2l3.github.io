<?php

namespace App\Models;

use Eloquent;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Support\Carbon;

/**
 * App\Models\LinkVisit
 *
 * @property int $id
 * @property int $link_id
 * @property string|null $platform
 * @property string|null $device
 * @property string|null $browser
 * @property string|null $location
 * @property int $crawler
 * @property string|null $referrer
 * @property string|null $ip
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property-read Link $link
 * @method static Builder|LinkVisit newModelQuery()
 * @method static Builder|LinkVisit newQuery()
 * @method static Builder|LinkVisit query()
 * @method static Builder|LinkVisit whereBrowser($value)
 * @method static Builder|LinkVisit whereCrawler($value)
 * @method static Builder|LinkVisit whereCreatedAt($value)
 * @method static Builder|LinkVisit whereDevice($value)
 * @method static Builder|LinkVisit whereId($value)
 * @method static Builder|LinkVisit whereIp($value)
 * @method static Builder|LinkVisit whereLinkId($value)
 * @method static Builder|LinkVisit whereLocation($value)
 * @method static Builder|LinkVisit wherePlatform($value)
 * @method static Builder|LinkVisit whereReferrer($value)
 * @method static Builder|LinkVisit whereUpdatedAt($value)
 * @mixin Eloquent
 */
class LinkVisit extends Model
{
    /**
     * Get user
     *
     * @return BelongsTo
     */
    public function link()
    {
        return $this->belongsTo('App\Models\Link');
    }
}
