<?php

namespace App\Support;

use App\Models\Link;
use App\Models\LinkVisit;
use Carbon\Carbon;
use Illuminate\Database\Query\Builder;
use Illuminate\Support\Collection;

class Analytics
{
    /**
     * @var string
     */
    private $property;

    /**
     * @var Builder
     */
    private $linkVisit;

    /**
     * Analytics constructor.
     * @param  string  $property
     * @param  Carbon  $start
     * @param  Carbon  $end
     */
    public function __construct(string $property, Carbon $start, Carbon $end)
    {
        $this->property = $property;
        $this->linkVisit = LinkVisit::selectRaw('count(*) AS count, '.$this->property)->groupBy($this->property);
        $this->linkVisit->whereBetween('created_at', [$start, $end]);
        $this->linkVisit->groupBy($this->property);
        $this->linkVisit->orderByDesc('count');
        return $this;
    }

    /**
     * @param  int  $link
     * @return $this
     */
    public function link(int $link)
    {
        $this->linkVisit->where('link_id', $link);
        return $this;
    }

    /**
     * @return $this
     */
    public function onlyUser()
    {
        $userLinks = Link::where('user_id', auth('api')->id())->pluck('id');
        $this->linkVisit->whereIn('link_id', $userLinks);
        return $this;
    }

    /**
     * @return $this
     */
    public function notNull()
    {
        $this->linkVisit->whereNotNull($this->property);
        $this->linkVisit->where($this->property, '!=', '');
        return $this;
    }

    /**
     * @param  int  $limit
     * @return $this
     */
    public function limit($limit = 10)
    {
        $this->linkVisit->limit($limit);
        return $this;
    }

    /**
     * @return Collection
     */
    public function get()
    {
        return $this->linkVisit->get();
    }
}
