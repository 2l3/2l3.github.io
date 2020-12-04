<?php

namespace App\Models;

use App\Transformers\LinkTransformer;
use Eloquent;
use EloquentFilter\Filterable;
use Flugg\Responder\Contracts\Transformable;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Collection;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Support\Carbon;

/**
 * App\Models\Link
 *
 * @property int $id
 * @property string $code
 * @property string $url
 * @property int $type
 * @property string|null $title
 * @property string|null $tags
 * @property string|null $description
 * @property string|null $password
 * @property int $archived
 * @property int $disabled
 * @property int|null $user_id
 * @property int|null $domain_id
 * @property Carbon|null $expires_at
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property-read Domain|null $domain
 * @property-read User|null $user
 * @property-read Collection|LinkVisit[] $visits
 * @property-read int|null $visits_count
 * @method static Builder|Link filter($input = [], $filter = null)
 * @method static Builder|Link newModelQuery()
 * @method static Builder|Link newQuery()
 * @method static Builder|Link paginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Link query()
 * @method static Builder|Link simplePaginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Link whereArchived($value)
 * @method static Builder|Link whereBeginsWith($column, $value, $boolean = 'and')
 * @method static Builder|Link whereCode($value)
 * @method static Builder|Link whereCreatedAt($value)
 * @method static Builder|Link whereDescription($value)
 * @method static Builder|Link whereDisabled($value)
 * @method static Builder|Link whereDomainId($value)
 * @method static Builder|Link whereEndsWith($column, $value, $boolean = 'and')
 * @method static Builder|Link whereExpiresAt($value)
 * @method static Builder|Link whereId($value)
 * @method static Builder|Link whereLike($column, $value, $boolean = 'and')
 * @method static Builder|Link wherePassword($value)
 * @method static Builder|Link whereTags($value)
 * @method static Builder|Link whereTitle($value)
 * @method static Builder|Link whereType($value)
 * @method static Builder|Link whereUpdatedAt($value)
 * @method static Builder|Link whereUrl($value)
 * @method static Builder|Link whereUserId($value)
 * @mixin Eloquent
 */
class Link extends Model implements Transformable
{
    use Filterable;

    /**
     * The attributes that should be cast to native types.
     *
     * @var array
     */
    protected $casts = [
        'expires_at' => 'datetime',
    ];

    /**
     * @inheritDoc
     */
    public function transformer()
    {
        return LinkTransformer::class;
    }

    /**
     * Get user
     *
     * @return BelongsTo
     */
    public function user()
    {
        return $this->belongsTo('App\Models\User');
    }

    /**
     * Get domain
     *
     * @return BelongsTo
     */
    public function domain()
    {
        return $this->belongsTo('App\Models\Domain');
    }

    /**
     * @return HasMany
     */
    public function visits()
    {
        return $this->hasMany('App\Models\LinkVisit');
    }

    /**
     * @return array
     */
    public function getTags()
    {
        return json_decode($this->tags);
    }

    /**
     * @return string
     */
    public function getFavicon()
    {
        return 'https://www.google.com/s2/favicons?domain='.parse_url($this->url)['host'];
    }

    /**
     * @return string
     */
    public function getLink()
    {
        if ($this->domain_id) {
            return url(str_replace('//', '/', str_replace('://', ':///', $this->domain->domain.'/'.$this->code)));
        } else {
            if ($domain = Domain::find(setting('app_domain'))) {
                return url(str_replace('//', '/', str_replace('://', ':///', $domain->domain.'/'.$this->code)));
            }
        }
        return url($this->code);
    }

    /**
     * @return string
     */
    public function getQr()
    {
        return $this->getLink().'/qr';
    }
}
