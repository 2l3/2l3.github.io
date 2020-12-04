<?php

namespace App\Filters;

use EloquentFilter\ModelFilter;
use Illuminate\Database\Eloquent\Builder;

class LinkFilter extends ModelFilter
{
    /**
     * Related Models that have ModelFilters as well as the method on the ModelFilter
     * As [relationMethod => [input_key1, input_key2]].
     *
     * @var array
     */
    public $relations = [];

    /**
     * Search filter
     *
     * @param $search
     * @return LinkFilter
     */
    public function search($search)
    {
        return $this->where(function ($query) use ($search) {
            /** @var Builder $query */
            return $query->where('code', 'LIKE', '%'.$search.'%')
                ->orWhere('url', 'LIKE', '%'.$search.'%')
                ->orWhere('title', 'LIKE', '%'.$search.'%')
                ->orWhere('description', 'LIKE', '%'.$search.'%');
        });
    }

    /**
     * Filter by disabled
     *
     * @param $status
     * @return LinkFilter
     */
    public function disabled($status)
    {
        return $this->where('disabled', (bool) $status);
    }

    /**
     * Filter by disabled
     *
     * @param $archived
     * @return LinkFilter
     */
    public function archived($archived)
    {
        return $this->where('archived', (bool) $archived);
    }
}
